using System;
using System.Collections.Generic;

namespace DBTesting.Models;

public partial class Employees
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Address { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public int? IdDepartament { get; set; }

    public virtual Department IdDepartamentNavigation { get; set; }
}