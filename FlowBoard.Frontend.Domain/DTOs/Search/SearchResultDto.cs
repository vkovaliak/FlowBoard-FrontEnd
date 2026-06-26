namespace FlowBoard.Frontend.Domain.DTOs.Search;

public record SearchResultDto(
    List<SearchBoardDto> Boards,
    List<SearchCardDto> Cards);