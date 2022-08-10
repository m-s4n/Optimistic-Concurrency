using App.Test.Models;
using ConcurrencyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Test.Controllers
{
    public class UrunController : Controller
    {
        private readonly AppDbContext _context;
        public UrunController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var urun = await _context.Urun.FindAsync(id);
            return View(urun);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Urun urun)
        {
            try
            {
                _context.Urun.Update(urun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Liste));
            }
            catch (DbUpdateConcurrencyException exception)
            {
                // eger böyle bir exception fırlatıldı ise ben ilgili satırı alayım
                //Birden fazla satır olabilir biz ilkini alıyoruz
                var entity = exception.Entries.First();

                var currentValues = entity.Entity as Urun;

                var databaseValues = entity.GetDatabaseValues();

                var clientValues = entity.CurrentValues.ToObject() as Urun;


                // null ise demekki silinmiş
                if (databaseValues == null)
                {
                    // Az önce silindi
                    ModelState.AddModelError(string.Empty, "Bu ürün silindi");
                }
                else
                {
                    var databaseUrun = databaseValues.ToObject() as Urun;
                    // Az önce güncellendi
                    ModelState.AddModelError(string.Empty, "Bu ürün başkası tarafından güncellendi");
                }
                return View(); // Tekrar View döndürülür.
            }
        }

        [HttpGet]
        public async Task<IActionResult> Liste()
        {

            return View(await _context.Urun.ToListAsync());
        }
    }
}
