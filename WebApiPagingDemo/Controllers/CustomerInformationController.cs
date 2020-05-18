using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApiPagingDemo.EFModel;
using WebApiPagingDemo.Models;

namespace WebApiPagingDemo.Controllers
{
    public class CustomerInformationController : ApiController
    {
        
        CustomerDBEntities _dbContext;
        public CustomerInformationController()
        {
            _dbContext = new CustomerDBEntities();
        }

        [HttpGet]
        public IEnumerable<CustomerTB> GetCustmomer([FromUri] PagingParameterModel pagingParameterModel)
        {
            var source = _dbContext.CustomerTBs.ToList();

            //Get Total source count
            int count = source.Count();

            //page number parameter passed from the query string if not provide default which we have already set as 1 
            int currentPage = pagingParameterModel.pageNumber;

            //page size parameter passed from the query string or default value which is 20.
            int pageSize = pagingParameterModel.pageSize;

            // Display TotalCount to Records to User
            int TotalCount = count;

            //Getting total Pages
            int TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            //// if CurrentPage is greater than 1 means it has previousPage 
            var previousPage = currentPage > 1 ? "Yes" : "NO";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = currentPage < TotalPages ? "Yes" : "No";


            //Object which we are going to send in header
            var paginationMetaData = new
            {
                TotalCount = TotalCount,
                pageSize = pageSize,
                currentPage = currentPage,
                TotalPages = TotalPages,
                previousPage,
                nextPage
            };


            //setting Header
            HttpContext.Current.Response.Headers.Add("Paging-Header", JsonConvert.SerializeObject(paginationMetaData));

            // Returing List of Customers Collections  
            return items;
        }


    }
}
