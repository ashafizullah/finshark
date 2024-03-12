using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
	public static class CommentMapper
	{
		public static CommentDto ToCommentDto(this Comment commentModel){
			return new CommentDto{
				Id = commentModel.Id,
				Title = commentModel.Title,
				Content = commentModel.Content,
				CreatedAt = commentModel.CreatedAt,
				StockId = commentModel.StockId
			};
		}

		public static Comment ToCommentFromCreateDto(this CreateCommentDto commentDto, int StockId){
			return new Comment{
				Title = commentDto.Title,
				Content = commentDto.Content,
				StockId = StockId
			};
		}

		public static Comment ToCommentFromUpdateDto(this UpdateCommentRequestDto commentDto){
			return new Comment{
				Title = commentDto.Title,
				Content = commentDto.Content
			};
		}
	}
}