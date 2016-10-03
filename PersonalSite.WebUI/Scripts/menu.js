var isMobile = false;
$(document).on("mobileinit", function () {
    isMobile = true;
    $.mobile.ignoreContentEnabled = true;
    $.mobile.ajaxEnabled = false;
    $.mobile.linkBindingEnabled = false;
});

$(document).ready(function () {
    if (!isMobile) {
        var topValue = '-' + $('div.menu-list').outerHeight() + 'px';
        $('div.menu-list').css({ top: topValue });
    }
    var Menu = {
        el: {
            ham: $('.menu-header'),
            menuTop: $('.menu-top'),
            menuMiddle: $('.menu-middle'),
            menuBottom: $('.menu-bottom')
        },

        init: function () {
            Menu.bindUIactions();
        },

        bindUIactions: function () {
            Menu.el.ham
                .on(
                  'click',
                function (event) {
                    Menu.activateMenu(event);
                    event.preventDefault();
                    if (isMobile) {
                        if ($('div.menu').find('div.menu-list').length > 0) {
                            $('div.menu-list').removeClass('menu-list').addClass('menu-list-open');
                        }
                        else {
                            if ($('div.menu').find('div.menu-list-open').length > 0 ) {
                                $('div.menu-list-open').removeClass('menu-list-open').addClass('menu-list');
                            }
                        }
                    }
                    else {
                        var topValue = '-' + $('div.menu-list').outerHeight() + "px";

                        if ($('div.menu-list').position().top == 0) {
                            $('div.menu-list').animate({ top: topValue }, 'slow');
                        }
                        else {
                            $('div.menu-list').animate({ top: '0' }, 'slow');
                        }
                    }
                }
            );
        },

        activateMenu: function () {
            Menu.el.menuTop.toggleClass('menu-top-click');
            Menu.el.menuMiddle.toggleClass('menu-middle-click');
            Menu.el.menuBottom.toggleClass('menu-bottom-click');
        }
    };

    Menu.init();
});
