using FlowBoard.Frontend.Domain.Enums;

namespace FlowBoard.Frontend.Domain.DTOs.Cards;
public record SetCardCoverDto(
    string? Color,
    CardCoverMode Mode);