window.initKanbanSortable = (dotNetHelper) => {
    const boardContainer = document.querySelector('.board-lists-container');
    if (boardContainer) {
        new Sortable(boardContainer, {
            group: 'lists',
            animation: 250,
            handle: '.list-drag-handle',
            draggable: '.list-draggable-item',
            ghostClass: 'sortable-ghost',

            swapThreshold: 0.7,
            invertSwap: true,

            onEnd: function (evt) {
                if (evt.oldIndex === evt.newIndex) 
                    return;

                const listId = evt.item.getAttribute('data-list-id');
                const newIndex = evt.newIndex;

                dotNetHelper.invokeMethodAsync('HandleListMovedJS', listId, newIndex);
            }
        });
    }
};