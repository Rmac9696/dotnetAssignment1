using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using DOTNET_lab4.Data;
using DOTNET_lab4.Models;
using DOTNET_lab4.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_lab4.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly SchoolCommunityContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "advertisements";
        public AdvertisementsController(SchoolCommunityContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<IActionResult> Index(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Communities");
            }

            var viewModel = new CommunityAdvertViewModel();
            viewModel.Community = await _context.Communities.Include(i => i.Adverts).ThenInclude(i => i.Advertisement).Where(i => i.ID == id).SingleAsync(); // find the community and include it's advertisements
            if (viewModel.Community == null)
            {
                return RedirectToAction("Index", "Communities");
            }

            viewModel.Advertisements = viewModel.Community.Adverts.Select(i => i.Advertisement); // assign all the advertisements attached to a community


            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? AdvertisementID, string? CommunityID)
        {
            if(AdvertisementID == null || CommunityID == null)
            {
                return RedirectToAction("Index","Communities");
            }

            var model = await _context.Adverts.FindAsync(AdvertisementID, CommunityID);

            if(model == null)
            {
                return RedirectToAction("Index", "Communities");
            }
            model.Advertisement = await _context.Advertisements.FindAsync(AdvertisementID);
            return View(model);

        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("AdvertisementID", "CommunityID")] CommunityAdvert ad)
        { 
            if (ad == null)
            {
                return RedirectToAction("Index", "Communities");
            }

            var image = await _context.Advertisements.FindAsync(ad.AdvertisementID); // check if advertisement exists
            var community = await _context.Communities.FindAsync(ad.CommunityID); // check if community exists
            if (image == null || community == null)
            {
                return RedirectToAction("Index", "Communities");
            }

            ad.Community = community;
            ad.Advertisement = image;

            BlobContainerClient containerClient;
            try
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            } catch (RequestFailedException e)
            {
                return View("Error");
            }
            try
            {
                var blockBlob = containerClient.GetBlobClient(image.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }
                _context.Adverts.Remove(ad);
                await _context.SaveChangesAsync();
                _context.Advertisements.Remove(image);
                await _context.SaveChangesAsync();
                
            } catch (RequestFailedException e)
            {
                return View("Error");
            }

            return RedirectToAction("Index", new { id = ad.CommunityID });

        }

        public IActionResult Upload(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Communities");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(string? id, IFormFile file)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Communities");
            }
            {
                BlobContainerClient containerClient;
                try
                {
                    containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                    containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                }
                catch (RequestFailedException e)
                {
                    containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                }
                try
                {

                    var blockBlob = containerClient.GetBlobClient(file.FileName);
                    if (await blockBlob.ExistsAsync())
                    {
                        await blockBlob.DeleteAsync();
                    }

                    using (var memorystream = new MemoryStream())
                    {
                        await file.CopyToAsync(memorystream);

                        memorystream.Position = 0;

                        await blockBlob.UploadAsync(memorystream);
                        memorystream.Close();
                    }

                    var image = new Advertisement();
                    image.Url = blockBlob.Uri.AbsoluteUri;
                    image.FileName = file.FileName;
                    _context.Advertisements.Add(image);
                    _context.SaveChanges();
                    var advert = new CommunityAdvert { AdvertisementID= image.ID, CommunityID = id };
                    _context.Adverts.Add(advert);
                    _context.SaveChanges();
                }
                catch (RequestFailedException e)
                {
                    Debug.WriteLine(e.ToString());
                }
                return RedirectToAction("Index", new { id });
            }


        }
    }
}
