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
    public class DtoCastlingMoveFactoryKS : BaseDtoMoveFactory<DtoCastlingMoveKS>
    {
        public override DtoType TargetDtoType => DtoType.CastlingMoveKS;
        public override MoveType TargetMoveType => MoveType.CastleKS; 

        public DtoCastlingMoveFactoryKS(IGameService gameService) : base(gameService) { }

        protected override Position GetFromPosition(DtoCastlingMoveKS dto) => dto.FromPos;

        protected override Move? FindMove(IEnumerable<Move> legalMoves, DtoCastlingMoveKS dto)
        {
            return legalMoves.FirstOrDefault(m =>
                m.ToPos == dto.ToPos && m.Type == MoveType.CastleKS);
        }

        protected override string GetErrorMessage() => "Нелегальна рокіровка.";

        public override IDtoMessage GetMoveToDTO(Move move)
            => new DtoCastlingMoveKS(move.FromPos, move.ToPos);
    }
}
