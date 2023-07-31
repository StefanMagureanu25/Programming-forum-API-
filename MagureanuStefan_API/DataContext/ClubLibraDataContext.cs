using MagureanuStefan_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagureanuStefan_API.DataContext
{
    public class ClubLibraDataContext : DbContext
    {
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<CodeSnippet> CodeSnippets { get; set; }
        public ClubLibraDataContext(DbContextOptions<ClubLibraDataContext> options) : base(options) { }
    }
}
