using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using Product_Catalog.models;

namespace Product_Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly connection db;
        private readonly IWebHostEnvironment env;

        public HomeController(connection db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        [HttpPost("add")]
        public async Task<IActionResult> create([FromForm] product model)
        {
            try {
                if (model.File == null || model.File.Length == 0)
                {
                    return BadRequest("file no found");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if(model.File.Length > 5 * 1024 * 1024)
                {
                    return BadRequest("file size is greter than 5MB");
                }
                var allowedextensions = new[] { ".jpg", ".jpeg", ".png", ".pdf"};
                var extension = Path.GetExtension(model.File.FileName).ToLower();
                if (!allowedextensions.Contains(extension))
                {
                    return BadRequest("only jpg,jpeg and png are allowed");
                }
                var fileupload = Path.Combine(env.WebRootPath, "uploadedfiles");
                if (!Directory.Exists(fileupload))
                    Directory.CreateDirectory(fileupload);
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                var filepath = Path.Combine(fileupload, filename);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
                var imageurl = $"{Request.Scheme}://{Request.Host}/uploadedfiles/{filename}";
                model.ImagePath = filename;
                await db.products.AddAsync(model);
                await db.SaveChangesAsync();
                return Ok(new
                {
                    Id = model.Id,
                    Productname = model.ProductName,
                    Price = model.Price,
                    Desc = model.Description,
                    File = model.File.FileName,
                    Image = imageurl,
                    Message = "data added successfully"
                });
            } catch (Exception ex) { 
            return StatusCode(500, new {Error = "runtime error", Message = ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> show()
        {
            var data = await db.products.Select(x => new
            {
                x.Id,
                x.ProductName,
                x.Price,
                x.Description,
                Image = $"{Request.Scheme}://{Request.Host}/uploadedfiles/{x.ImagePath}",
            }).ToListAsync();
            return Ok(data);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> update([FromRoute]int id,[FromForm] product model)
        {
            var data = await db.products.FindAsync(id);
            if (data == null) return NotFound();
            if(model.File!=null && model.File.Length > 0)
            {
                if (model.File.Length > 5 * 1024 * 1024)
                {
                    return BadRequest("file is greter than 5MB");
                }
                var allowedextensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                var extension = Path.GetExtension(model.File.FileName).ToLower();
                if (!allowedextensions.Contains(extension))
                {
                    return BadRequest("only jpg,jpeg and png are allowed");
                }
                var oldpath = Path.Combine(env.WebRootPath, "uploadedfiles", data.ImagePath);
                if (System.IO.File.Exists(oldpath))
                    System.IO.File.Delete(oldpath);
                var newpath = Path.Combine(env.WebRootPath, "uploadedfiles");
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                var filepath = Path.Combine(newpath, filename);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
                data.ImagePath = filename;
            }
            data.ProductName = model.ProductName;
            data.Price = model.Price;
            data.Description = model.Description;
            await db.SaveChangesAsync();
            return Ok(new {Message="data updated successfully"});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete([FromRoute] int id)
        {
            var data = await db.products.FindAsync(id);
            if (data == null) return NotFound();
            var oldpath = Path.Combine(env.WebRootPath, "uploadedfiles",data.ImagePath);
            if (System.IO.File.Exists(oldpath))
                System.IO.File.Delete(oldpath);
            db.products.Remove(data);
            await db.SaveChangesAsync();
            return Ok(new {Message="data deleted successfully"});
        }
    }
}
