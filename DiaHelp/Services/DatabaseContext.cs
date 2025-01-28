using DiaHelp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.Services
{
    public class DatabaseContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
    }
}
