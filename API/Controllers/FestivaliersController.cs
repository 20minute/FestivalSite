using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;

namespace API.Controllers
{
    public class FestivaliersController : ApiController
    {
        private APIContext db = new APIContext();

        // GET: api/Festivaliers
        public IQueryable<Festivalier> GetFestivaliers()
        {
            return db.Festivaliers;
        }

        // GET: api/Festivaliers/5
        [ResponseType(typeof(Festivalier))]
        public async Task<IHttpActionResult> GetFestivalier(int id)
        {
            Festivalier festivalier = await db.Festivaliers.FindAsync(id);
            if (festivalier == null)
            {
                return NotFound();
            }

            return Ok(festivalier);
        }

        // PUT: api/Festivaliers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFestivalier(int id, Festivalier festivalier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != festivalier.ID)
            {
                return BadRequest();
            }

            db.Entry(festivalier).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FestivalierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Festivaliers
        [ResponseType(typeof(Festivalier))]
        public async Task<IHttpActionResult> PostFestivalier(Festivalier festivalier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Festivaliers.Add(festivalier);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = festivalier.ID }, festivalier);
        }

        // DELETE: api/Festivaliers/5
        [ResponseType(typeof(Festivalier))]
        public async Task<IHttpActionResult> DeleteFestivalier(int id)
        {
            Festivalier festivalier = await db.Festivaliers.FindAsync(id);
            if (festivalier == null)
            {
                return NotFound();
            }

            db.Festivaliers.Remove(festivalier);
            await db.SaveChangesAsync();

            return Ok(festivalier);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FestivalierExists(int id)
        {
            return db.Festivaliers.Count(e => e.ID == id) > 0;
        }
    }
}