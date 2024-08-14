using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using API.Extensions;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // public async Task<MemberDto> GetMemberAsync(string username)
        // {
        //     var member = await _context.Users
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
        //     return await _context.Users
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

        // public async Task<MemberDto> GetMemberAsync(string username)
        // {
        //     var member = await _context.Users
        //         .Where(x => x.UserName == username)
        //         .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
        //         .SingleOrDefaultAsync();

        //     if (member == null)
        //     {
        //         throw new InvalidOperationException("Member not found.");
        //     }

        //     return member;
        // }

        // public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        // {
        //     return await _context.Users
        //     .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
        //     .ToListAsync();
        // }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            var member = await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (member == null)
            {
                throw new InvalidOperationException("Member not found.");
            }

            return member;
        }

        // public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        // {
        //     return await _context.Users
        //         .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
        //         .ToListAsync();
        // }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            var users = await _context.Users
                .Include(u => u.Photos)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MemberDto>>(users);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return user;        
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
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
            return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}