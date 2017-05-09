
(function ($) {
    $.fn.paginatearticle = function (options) {
        var defaults = {
            pagercontainerSelector: null,
            nextSelector: null,
            prevSelector: null,
            pagerSelector: null,
            listcontainerSelector: null,
            itemslistSelector: null,
            currentpage: 1
        }

        var settings = $.extend({}, defaults, options);
        var base = this;

        function navigateTo(page) {
            var slidepx = $(settings.itemslistSelector).width();
            if (settings.currentpage > page) {
                // slide to right
                base.animate({ marginLeft: "+=" + (settings.currentpage - page) * slidepx }, { duration: 'slow' });
            }
            else {
                if (settings.currentpage < page) {
                    // slide to left
                    base.animate({ marginLeft: "-=" + (page - settings.currentpage) * slidepx }, { duration: 'slow' });
                }
            }

            settings.currentpage = page;
            if (settings.pagerSelector) {
                $(settings.pagerSelector).children('span').first().html(settings.currentpage);
            }
        }

        function setOnClickEvents() {
            if (settings.nextSelector) {
                $(settings.nextSelector).on('click', function () {
                    if (settings.currentpage < base.children('li').length) {
                        navigateTo(settings.currentpage + 1);
                    }
                    else {
                        base.css('color', 'black');
                    }
                });
            }

            if (settings.prevSelector) {
                $(settings.prevSelector).on('click', function () {
                    if (settings.currentpage > 1) {
                        navigateTo(settings.currentpage - 1);
                    }
                    else {
                        base.css('color', 'black');
                    }
                });
            }
        }   

        function setWidthPage() {
            var maxHeight = 0;
            var liWidth = $(settings.itemslistSelector).width();
            base.children('li').each(function () {
                $(this).css('width', liWidth + 'px');
                if ($(this).outerHeight(false) > maxHeight) {
                    maxHeight = $(this).outerHeight(false);
                }
            });

            $(settings.itemslistSelector).css('height', maxHeight + 10 + 'px');
            $(settings.listcontainerSelector).css('height', maxHeight + 10 + 'px');
            $(settings.pagercontainerSelector).css('width', liWidth + 'px');

            var ulWidth = base.children('li').length * liWidth;
            base.css('width', ulWidth + 'px');
        }

        this.init = function () {
            setWidthPage();
            setWidthPage();
            setOnClickEvents();
        }

        return this.each(function () {
            base.init();
        });
    }
}(jQuery));