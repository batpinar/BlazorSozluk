using BlazorSozluk.Api.Applicaition.Interfaces.Repositories;
using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Infrastructure.Persistance.Repositories;

public class EntryCommentRepository : GenericRepository<EntryComment>, IEntryCommentRepository
{
    public EntryCommentRepository(BlazorSozlukDbContext dbContext) : base(dbContext)
    {
    }
}
