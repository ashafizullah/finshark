using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
	[Route("api/stock")]
	[ApiController]
	public class StockController : ControllerBase
	{
		private readonly IStockRepository _stockRepo;
		public StockController(IStockRepository stockRepository)
		{
			_stockRepo = stockRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] QueryObject query){
			if(!ModelState.IsValid) return BadRequest();

			var stocks = await _stockRepo.GetAllAsync(query);

			var stockDto = stocks.Select(s => s.ToStockDto());

			return Ok(stockDto);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById([FromRoute] int id){
			if(!ModelState.IsValid) return BadRequest();

			var stock = await _stockRepo.GetByIdAsync(id);

			if(stock == null){
				return NotFound();
			}

			return Ok(stock.ToStockDto());
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){
			if(!ModelState.IsValid) return BadRequest();

			var stockModel = stockDto.ToStockFromCreateDto();

			await _stockRepo.CreateAsync(stockModel);

			return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
		}

		[HttpPut]
		[Route("{id:int}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto){
			if(!ModelState.IsValid) return BadRequest();

			var stockModel = await _stockRepo.UpdateAsync(id, updateDto);

			if(stockModel == null){
				return NotFound();
			}

			return Ok(stockModel.ToStockDto());
		}

		[HttpDelete]
		[Route("{id:int}")]
		public async Task<IActionResult> Delete([FromRoute] int id){
			if(!ModelState.IsValid) return BadRequest();

			var stockModel = await _stockRepo.DeleteAsync(id);

			if(stockModel == null){
				return NotFound();
			}

			return NoContent();
		}
	}
}