using System;
using System.Collections.Generic;

namespace GoldDiggerWebApi.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string ContactTitle { get; set; } = null!;

    public string Adresse { get; set; } = null!;

    public string Zip { get; set; } = null!;

    public string City { get; set; } = null!;

    public int CountryId { get; set; }

    public string PhoneNo { get; set; } = null!;

    public string MailAdr { get; set; } = null!;

    public bool Activ { get; set; }
}
