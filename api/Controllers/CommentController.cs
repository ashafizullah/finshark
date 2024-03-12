using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	[Route("api/comment")]
	[ApiController]
	public class CommentController : ControllerBase
	{
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;
		public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
		{
      _stockRepo = stockRepo;
      _commentRepo = commentRepo;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll(){
			if(!ModelState.IsValid) return BadRequest();

			var comments = await _commentRepo.GetAllAsync();

			var commentDto = comments.Select(s => s.ToCommentDto());

			return Ok(commentDto);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById([FromRoute] int id){
			if(!ModelState.IsValid) return BadRequest();

			var comment = await _commentRepo.GetByIdAsync(id);

			if(comment == null){
				return NotFound();
			}

			return Ok(comment.ToCommentDto());
		}

		[HttpPost("{stockId:int}")]
		public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto){
			if(!ModelState.IsValid) return BadRequest();

			if(!await _stockRepo.StockExists(stockId)){
				return BadRequest("Stock does not exist");
			}

			var commentModel = commentDto.ToCommentFromCreateDto(stockId);

			await _commentRepo.CreateAsync(commentModel);

			return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
		}

		[HttpDelete]
		[Route("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id){
			if(!ModelState.IsValid) return BadRequest();

			var commentModel = await _commentRepo.DeleteAsync(id);

			if(commentModel == null){
				return NotFound("Comment does not exist");
			}

			return Ok(commentModel);
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto){
			var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdateDto());

			if(comment == null){
				return NotFound("Comment not found");
			}

			return Ok(comment.ToCommentDto());
		}
	}
}