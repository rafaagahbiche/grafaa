(function ($) {
    $.fn.pagearticle.save = function (options) {
        var defaults = {
            loadingSelector: null,

        }
        var base = this;

        this.init = function () {
        }

        return this.each(function () {
            base.init();
        });
    }
});