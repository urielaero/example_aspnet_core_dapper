using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;


using dapperOdata.Models;

namespace dapperOdata.Controllers
{


    public class BooksController : ODataController
    {

    	private readonly Repo _repo;
        public BooksController(Repo repo) {
            _repo = repo;
        }

        public IActionResult Get([FromODataUri] int key)
        {
            var book = _repo.Get(new BookFast{Id=key});
            if (book == null) {
                return NotFound();
            }
	        return Ok(book);
        }

        //[EnableQuery(PageSize=2)]
        public IActionResult Get() //ODataQueryOptions<BookFast> query
        {
	        return Ok(_repo.Find<BookFast>());
        }

        public IActionResult Post(BookFast book)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            _repo.Insert(book);
            return Created("books", book);
        }

        public static void WriteObj<T>(T obj)
        {
            Console.WriteLine("LOG");
            var t = typeof(T);
            var props = t.GetProperties();
            StringBuilder sb = new StringBuilder();
            foreach (var item in props)
            {
                sb.Append($"{item.Name}:{item.GetValue(obj,null)}; ");
            }
            sb.AppendLine();
            Console.WriteLine(sb.ToString());
            Console.WriteLine("LOGEND");
        }
    }

}
