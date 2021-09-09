using ChessAPI.Models;
using ChessEngine.GameHandle;
using Microsoft.AspNetCore.Mvc;

namespace ChessAPI.Controllers
{
    [Route("[controller]")]
    public class ChessController : Controller
    {
        public ChessController(){}

        [HttpGet("GetNextMove")]
        public ActionResult<Move> GetNextMove(string fen)
        {
            var handler = new GameHandler();
            handler.InitializeGame(fen);
            var move = handler.GetNextMove();
            handler.SendMove(move);
            var response = new Move()
            {
                MoveUCI = move,
                FEN = handler.GetFEN() + " 0 0"
            };
            return Ok(response);
        }

        [HttpGet("GetFenAfterMove")]
        public ActionResult<Move> GetFenAfterMove(string fen, string move)
        {
            var handler = new GameHandler();
            handler.InitializeGame(fen);
            handler.SendMove(move);
            var response = new Move()
            {
                FEN = handler.GetFEN() + " 0 0"
            };
            return Ok(response);
        }
    }
}