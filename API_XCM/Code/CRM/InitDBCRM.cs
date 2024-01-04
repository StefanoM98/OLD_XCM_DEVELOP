using API_XCM.Models.XCM.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Code.CRM
{
    public class InitDBCRM
    {

        public static void InitCustomerTable()
        {
            XCM_CRMEntities entity = new XCM_CRMEntities();
            List<CustomerEspritecAPI> customers = EspritecAPI_XCM.CommonCustomerList();

            foreach (var c in customers)
            {
                var exsist = entity.Customer.FirstOrDefault(x => x.Customer_id == c.id);
                if (exsist == null)
                {
                    var newCustomer = new Customer()
                    {
                        Customer_id = c.id,
                        Customer_description = c.description,
                        Customer_isEnable = c.isEnable,
                        Customer_address = c.address,
                        Customer_zipCode = c.zipCode,
                        Customer_location = c.location,
                        Customer_district = c.district,
                        Customer_country = c.country,
                        Customer_defaultPriceListId = c.defaultPriceListId,
                        Customer_CreationDate = DateTime.Now,
                        Customer_LastModifiedDate = DateTime.Now,
                        Customer_LastModifiedUserID = "999",
                        Customer_vatCode = c.vatCode,
                        Customer_IsEnableCRM = true,
                        Customer_SessionID = Guid.NewGuid().ToString(),
                    };

                    entity.Customer.Add(newCustomer);
                    entity.SaveChanges();
                }
                else
                {
                    exsist.Customer_id = c.id;
                    exsist.Customer_description = c.description;
                    exsist.Customer_isEnable = c.isEnable;
                    exsist.Customer_address = c.address;
                    exsist.Customer_zipCode = c.zipCode;
                    exsist.Customer_location = c.location;
                    exsist.Customer_district = c.district;
                    exsist.Customer_country = c.country;
                    exsist.Customer_defaultPriceListId = c.defaultPriceListId;
                    exsist.Customer_LastModifiedDate = DateTime.Now;
                    exsist.Customer_LastModifiedUserID = "999";
                    exsist.Customer_vatCode = c.vatCode;
                    entity.SaveChanges();
                }

            }
        }

    }
}