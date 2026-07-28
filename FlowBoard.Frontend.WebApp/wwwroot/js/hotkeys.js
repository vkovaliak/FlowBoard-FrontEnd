window.hotkeys = {
    dotNetRef: null,

    register: function (dotNet) {
        this.dotNetRef = dotNet;
        this._handler = this._onKeyDown.bind(this);
        document.addEventListener('keydown', this._handler);
    },

    unregister: function () {
        if (this._handler) {
            document.removeEventListener('keydown', this._handler);
        }
        this.dotNetRef = null;
    },

    focusSearch: function () {
        const input = document.querySelector('.search-input input');
        if (input) {
            input.focus();
        }
    },

    isSearchFocused: function () {
        const input = document.querySelector('.search-input input');
        return input !== null && document.activeElement === input;
    },

    unFocusSearch: function () {
        const input = document.querySelector('.search-input input');
        if (input) {
            input.blur();
        }
    },

    _onKeyDown: function (e) {
        if (this.dotNetRef === null) return;

        const tag = document.activeElement?.tagName?.toLowerCase();
        const isEditable = tag === 'input'
            || tag === 'textarea'
            || document.activeElement?.isContentEditable;

        if (e.key === 'Escape') {
            this.dotNetRef.invokeMethodAsync('OnHotkey', 'Escape');
            return;
        }

        if (isEditable) return;
        if (e.ctrlKey || e.altKey || e.metaKey) return;

        let handled = true;
        switch (e.key) {
            case '/':
                this.dotNetRef.invokeMethodAsync(
                    'OnHotkey', 'Search');
                break;
            case 'n':
            case 'N':
                this.dotNetRef.invokeMethodAsync(
                    'OnHotkey', 'Notifications');
                break;
            case 'a':
            case 'A':
                this.dotNetRef.invokeMethodAsync(
                    'OnHotkey', 'Chat');
                break;
            case '?':
                this.dotNetRef.invokeMethodAsync(
                    'OnHotkey', 'Help');
                break;
            default:
                handled = false;
        }

        if (handled) e.preventDefault();
    }
};