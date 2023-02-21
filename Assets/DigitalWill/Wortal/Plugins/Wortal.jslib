mergeInto(LibraryManager.library, {

    OnPauseJS: function (callback) {
        window.Wortal.onPause(() => Module.dynCall_v(callback));
    }

});
