using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using API.Extensions;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.Helpers;

namespace API.Data
{
    public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
    {


        // public async Task<MemberDto> GetMemberAsync(string username)
        // {
        //     var member = await context.Users
        //         .Where(x => x.UserName == username)
        //         .Include(u => u.Photos)
        //         .Select(u => new 
        //         {
        //             User = u,
        //             MainPhoto = u.Photos.FirstOrDefault(p => p.IsMain)
        //         })
        //         .Select(u => new MemberDto
        //         {
        //             Id = u.User.Id,
        //             UserName = u.User.UserName,
        //             PhotoUrl = u.MainPhoto != null ? u.MainPhoto.Url : null, // Handle null safely
        //             Age = u.User.DateOfBirth.CalculateAge(),
        //             KnownAs = u.User.KnownAs,
        //             Created = u.User.Created,
        //             LastActive = u.User.LastActive,
        //             Gender = u.User.Gender,
        //             Introduction = u.User.Introduction,
        //             LookingFor = u.User.LookingFor,
        //             Interests = u.User.Interests,
        //             City = u.User.City,
        //             Country = u.User.Country,
        //             Photos = u.User.Photos.Select(p => new PhotoDto
        //             {
        //                 Id = p.Id,
        //                 Url = p.Url,
        //                 IsMain = p.IsMain
        //             }).ToList()
        //         })
        //         .SingleOrDefaultAsync();

        //     if (member == null)
        //     {
        //         throw new InvalidOperationException("Member not found.");
        //     }

        //     return member;
        // }

        // public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        // {
        //     return await context.Users
        //         .Include(u => u.Photos)
        //         .Select(u => new 
        //         {
        //             User = u,
        //             MainPhoto = u.Photos.FirstOrDefault(p => p.IsMain)
        //         })
        //         .Select(u => new MemberDto
        //         {
        //             Id = u.User.Id,
        //             UserName = u.User.UserName,
        //             PhotoUrl = u.MainPhoto != null ? u.MainPhoto.Url : null, // Handle null safely
        //             Age = u.User.DateOfBirth.CalculateAge(),
        //             KnownAs = u.User.KnownAs,
        //             Created = u.User.Created,
        //             LastActive = u.User.LastActive,
        //             Gender = u.User.Gender,
        //             Introduction = u.User.Introduction,
        //             LookingFor = u.User.LookingFor,
        //             Interests = u.User.Interests,
        //             City = u.User.City,
        //             Country = u.User.Country,
        //             Photos = u.User.Photos.Select(p => new PhotoDto
        //             {
        //                 Id = p.Id,
        //                 Url = p.Url,
        //                 IsMain = p.IsMain
        //             }).ToList()
        //         })
        //         .ToListAsync();
        // }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            var member = await context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (member == null)
            {
                throw new InvalidOperationException("Member not found.");
            }

            return member;
        }

        // public async Task<MemberDto> GetMemberAsync(string username)
        // {
        //     var user = await context.Users
        //         .Include(p => p.Photos)
        //         .SingleOrDefaultAsync(x => x.UserName == username);

        //     if (user == null)
        //     {
        //         throw new InvalidOperationException("User not found.");
        //     }

        //     return mapper.Map<MemberDto>(user);
        // }

        // public async Task<IEnumerable<MemberDto>> GetMemberAsync()
        // {
        //     return await context.Users
        //     .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
        //     .ToListAsync();
        // }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable();

            query = query.Where(x => x.UserName != userParams.CurrentUsername);

            if(userParams.Gender != null) {
                query = query.Where(x => x.Gender == userParams.Gender);
            }

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge-1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };
                
            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return user;        
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var user = await context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return user;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}