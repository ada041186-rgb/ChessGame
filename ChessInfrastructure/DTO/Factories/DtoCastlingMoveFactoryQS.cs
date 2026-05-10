using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessLibrary.Enums;
using ChessLibrary.Moves;
using ChessLibrary.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessInfrastructure.DTO.Factories
{
    public class DtoCastlingMoveFactoryQS : BaseDtoMoveFactory<DtoCastlingMoveQS>
    {
        public override DtoType TargetDtoType => DtoType.CastlingMoveQS;
        public override MoveType TargetMoveType => MoveType.CastleQS; 

        public DtoCastlingMoveFactoryQS(IGameService gameService) : base(gameService) { }

        protected override Position GetFromPosition(DtoCastlingMoveQS dto) => dto.FromPos;

        protected override Move? FindMove(IEnumerable<Move> legalMoves, DtoCastlingMoveQS dto)
        {
            return legalMoves.FirstOrDefault(m =>
                m.ToPos == dto.ToPos && m.Type == MoveType.CastleQS); 
        }

        protected override string GetErrorMessage() => "Нелегальна рокіровка.";

        public override IDtoMessage GetMoveToDTO(Move move)
            => new DtoCastlingMoveQS(move.FromPos, move.ToPos);
    }
}
