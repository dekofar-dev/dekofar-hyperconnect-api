using AutoMapper;
using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Application.Features.Tags.Commands;
using Dekofar.HyperConnect.Application.Features.Tags.DTO;
using Dekofar.HyperConnect.Domain.Entities.Orders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Dekofar.HyperConnect.Application.Tags.Commands
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, TagDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTagCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var entity = new Tag
            {
                Name = request.Name.Trim()
            };

            _context.Tags.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TagDto>(entity);
        }
    }
}
