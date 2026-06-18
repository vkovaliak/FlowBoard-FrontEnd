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

            onStart: function (evt) {
                evt.item._nextSibling = evt.item.nextElementSibling;
                evt.item._parent = evt.item.parentNode;
            },

            onEnd: function (evt) {
                if (evt.oldIndex === evt.newIndex) 
                    return;

                const listId = evt.item.getAttribute('data-list-id');
                const newIndex = evt.newIndex;

                undoDrop(evt)

                dotNetHelper.invokeMethodAsync('HandleListMovedJS', listId, newIndex);
            }
        });
    }

    const cardContainers = document.querySelectorAll('.cards-drop-zone');
    cardContainers.forEach(container => {
        new Sortable(container, {
            group: 'cards',
            animation: 200,
            ghostClass: 'sortable-ghost',
            swapThreshold: 0.75,
            invertSwap: true,

            onStart: function (evt) {
                evt.item._nextSibling = evt.item.nextElementSibling;
                evt.item._parent = evt.item.parentNode;
            },

            onEnd: function (evt) {
                const cardId = evt.item.getAttribute('data-card-id');
                const toListId = evt.to.getAttribute('data-list-id');
                const newIndex = evt.newIndex;

                undoDrop(evt)

                dotNetHelper.invokeMethodAsync(
                    'HandleCardMovedJS', cardId, toListId, newIndex);
            }
        });
    });
};

function undoDrop(evt) {
    const item = evt.item;
    const parent = item._parent;
    const nextSibling = item._nextSibling;

    if (!parent) return;

    if (nextSibling && nextSibling.parentNode === parent) {
        parent.insertBefore(item, nextSibling);
    } else {
        parent.appendChild(item);
    }

    delete item._nextSibling;
    delete item._parent;
}