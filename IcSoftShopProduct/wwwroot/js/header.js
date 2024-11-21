document.addEventListener('DOMContentLoaded', function () {
    var closeButton = document.querySelector('.close');
    var headerTop = document.querySelector('.header__top');

    closeButton.addEventListener('click', function () {

        headerTop.classList.add('hidden');

    
        setTimeout(function () {
            headerTop.style.display = 'none';
        }, 300); 
    });
});


document.addEventListener('DOMContentLoaded', function () {

    const collectionLinks = document.querySelectorAll('.header__menu li#bosuutap .dropdown li a');


    collectionLinks.forEach(function (link) {
      
        if (link.href === window.location.href) {
          
            const parentLi = document.querySelector('#bosuutap');

 
            if (parentLi) {
                parentLi.classList.add('active');
            }
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const categorylinks = document.querySelectorAll('.header__menu li#sanpham .dropdown li a');
    categorylinks.forEach(function (link) {
        if (link.href === window.location.href) {
            const parentLi = document.querySelector('#sanpham');
            if (parentLi) {
                parentLi.classList.add('active');
            }
        }

    });
});

document.addEventListener('DOMContentLoaded', function () {
    const shopall = document.querySelector('.header__menu li#sanpham a#shopall');
    if (shopall.href === window.location.href) {
        const parentLi = document.querySelector('#sanpham');
        if (parentLi) {
            parentLi.classList.add('active');
        }
    }

});

document.addEventListener('DOMContentLoaded', function () {
    const shopall = document.querySelector('.header__menu li#sale a#shopsale');
    if (shopall.href === window.location.href) {
        const parentLi = document.querySelector('#sale');
        if (parentLi) {
            parentLi.classList.add('active');
        }
    }

});



document.addEventListener('DOMContentLoaded', function () {
    var menuIcon = document.querySelector('.menu-icon');
    var slideInMenu = document.getElementById('slideInMenu');
    var overlay = document.createElement('div');
    var productsMenu = document.getElementById('productsMenu');
    var collectionsMenu = document.getElementById('collectionsMenu');
    overlay.className = 'menu-overlay';
    document.body.appendChild(overlay);

    menuIcon.addEventListener('click', function (e) {
        e.preventDefault();  // Prevent default link behavior
        slideInMenu.classList.toggle('active');
        overlay.classList.toggle('active');
    });

    overlay.addEventListener('click', function () {
        slideInMenu.classList.remove('active');
        productsMenu.classList.remove('active');
        collectionsMenu.classList.remove('active');
        overlay.classList.remove('active');
    });
});



document.addEventListener('DOMContentLoaded', function () {
    var showProductsMenu = document.getElementById('showProductsMenu');
    var showCollectionsMenu = document.getElementById('showCollectionsMenu');
    var productsMenu = document.getElementById('productsMenu');
    var collectionsMenu = document.getElementById('collectionsMenu');
    var mainMenu = document.getElementById('slideInMenu');
    var backToMainMenu = document.getElementById('backToMainMenu');
    var backToMainMenuFromCollections = document.getElementById('backToMainMenuFromCollections');


    showProductsMenu.addEventListener('click', function (e) {
        e.preventDefault();
        mainMenu.classList.remove('active');
        productsMenu.classList.add('active');
    });

    showCollectionsMenu.addEventListener('click', function (e) {
        e.preventDefault();
        mainMenu.classList.remove('active');
        collectionsMenu.classList.add('active');
    });

    backToMainMenu.addEventListener('click', function (e) {
        e.preventDefault();
        productsMenu.classList.remove('active');
        mainMenu.classList.add('active');
    });


    backToMainMenuFromCollections.addEventListener('click', function (e) {
        e.preventDefault();
        collectionsMenu.classList.remove('active');
        mainMenu.classList.add('active');
    });
});

$(document).ready(function () {
    var overlay = document.createElement('div');
    overlay.className = 'menu-overlay';
    document.body.appendChild(overlay);
    // Khi click vào menu-icon search
    $('.menu-icon#search').on('click', function (e) {
        console.log(e);
        e.preventDefault();


        $('.desktop-menu, .mobile-menu').addClass('hidden');
        overlay.classList.toggle('active');


        $('.search-custom').addClass('active');
    });

    // Khi nhấn lại để đóng custom-search
    $('.menu-icon#search-custom').on('click', function (e) {
        e.preventDefault();

        $('.desktop-menu, .mobile-menu').removeClass('hidden');
        overlay.classList.remove('active');

        $('.search-custom').removeClass('active');
    });

    $('.menu-overlay').on('click', function (e) {
        console.log(e);

        $('.desktop-menu, .mobile-menu').removeClass('hidden');
        $(this).removeClass('active');
        $('.search-custom').removeClass('active');
    });
});


$(document).ready(function () {

});

