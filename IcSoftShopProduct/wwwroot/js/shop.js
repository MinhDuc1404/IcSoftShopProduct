
function loadShop(page) {
    $.ajax({
        url: '/shopall/paging',
        method: 'GET',
        data: { page: page },
        success: function (response) {
            $('#product-list').empty();
            response.products.forEach(function (product) {
                var createdDate = new Date(product.createDate);
                var now = new Date();
                var timeDifferenceInMillis = now - createdDate;
                var timeDifferenceInDays = timeDifferenceInMillis / (1000 * 3600 * 24);
                var ProductSale = product.productPrice - (product.productPrice * product.productSale) / 100;
                var isNewArrival = timeDifferenceInDays <= 3;
                var isSale = product.productSale > 0;
                var HeaderImageUrl = product.productImage && product.productImage.length > 0 ? product.productImage[0].imageUrl : 'path/to/default-image.jpg';
                var formattedPrice = new Intl.NumberFormat('vi-VN').format(product.productPrice) + ' ₫';
                var formattedSalePrice = new Intl.NumberFormat('vi-VN').format(ProductSale) + ' ₫';
                console.log(domainCms);
                var productHTML = `
                                <div class="col-lg-4 col-md-6 col-sm-6 col-6">
                                    <div class="product__item__shop">
                                        <div class="product__item__pic__shop" style="background-image:url('${domainCms}/${HeaderImageUrl}');">
                                            <div class="overlay">
                                                <div class="product__info">
                                                    <a href="/${product.productName.replace(/ /g, '-').toLowerCase()}">
                                                        <h3 class="product__name">${product.productName}</h3>
                                                    </a>
                                                ${isSale ? ` <p class="product__price__sale">${formattedPrice}</p>
                                                             <p class="product__price">${formattedSalePrice}</p> ` :
                        ` <p class="product__price">${formattedPrice}</p>`}
                                                </div>
                                            </div>
                                             ${isNewArrival ? `<div class="new-arrival-badge">New Arrival</div>` : ''}
                                             ${isSale ? `<div class="sale-badge">-${product.productSale}%</div>` : ''}
                                        </div>
                                    </div>
                                </div>
                            `;
                $('#product-list').append(productHTML);
            });

            var filterPaging = document.getElementById('pagination');
            filterPaging.innerHTML = '';
            for (var i = 1; i <= response.totalPages; i++) {
                var pageLink = `<a href="javascript:void(0);" class="${i === response.currentPage ? "active" : ""} " onclick="loadShop(${i})">${i}</a>`;
                filterPaging.innerHTML += pageLink;
            }
        },
        error: function (err) {
            console.error("Failed to load products:", err);
        }
    });
}





var selectedPriceRange = null;
function filterProducts(priceRange, el) {

    selectedPriceRange = priceRange;
    console.log(el);

    const linksmobile = document.querySelectorAll('.card-heading a');

    linksmobile.forEach(link => link.classList.remove('active'));

    el.classList.add('active');

    const linksdesktop = document.querySelectorAll('.shop__sidebar__price a');

    linksdesktop.forEach(link => link.classList.remove('active'));

    el.classList.add('active');



    if (window.innerWidth <= 768) {
        toggleFilter();
    }
    applySorting(1);
}

function applySorting(pageNumber) {
    var sortOption = $('#sort-options').val();

    $.ajax({
        url: '/Shop/filter',
        type: 'GET',
        data: {
            priceRange: selectedPriceRange,
            sortOption: sortOption,
            pageNumber: pageNumber
        },
        success: function (response) {
            if (response.success && Array.isArray(response.products)) {

                $('#product-list').empty();

                var paging = document.getElementById('pagination');
                var filterPaging = document.getElementById('filter-pagination');

                    paging.style.display = 'none';
                    filterPaging.style.display = 'block';

  
                    filterPaging.innerHTML = '';
                    for (var i = 1; i <= response.totalPages; i++) {
                        var pageLink = `<a href="javascript:void(0);" class="${i === response.currentPage ? "active" : ""}" onclick="applySorting(${i})">${i}</a>`;
                        filterPaging.innerHTML += pageLink;
                    }
     


                response.products.forEach(function (product) {

                    var createdDate = new Date(product.createDate);
                    var now = new Date();
                    var timeDifferenceInMillis = now - createdDate;
                    var timeDifferenceInDays = timeDifferenceInMillis / (1000 * 3600 * 24);
                    var ProductSale = product.productPrice - (product.productPrice * product.productSale) / 100;
                    var isNewArrival = timeDifferenceInDays <= 3;
                    var isSale = product.productSale > 0;
                    var HeaderImageUrl = product.productImage && product.productImage.length > 0 ? product.productImage[0].imageUrl : 'path/to/default-image.jpg';
                    var formattedPrice = new Intl.NumberFormat('vi-VN').format(product.productPrice) + ' ₫';
                    var formattedSalePrice = new Intl.NumberFormat('vi-VN').format(ProductSale) + ' ₫';

                    var productHTML = `
                                <div class="col-lg-4 col-md-6 col-sm-6 col-6">
                                    <div class="product__item__shop">
                                        <div class="product__item__pic__shop" style="background-image:url('${domainCms}/${HeaderImageUrl}');">
                                            <div class="overlay">
                                                <div class="product__info">
                                                    <a href="/${product.productName.replace(/ /g, '-').toLowerCase()}">
                                                        <h3 class="product__name">${product.productName}</h3>
                                                    </a>
                                                ${isSale ? ` <p class="product__price__sale">${formattedPrice}</p>
                                                             <p class="product__price">${formattedSalePrice}</p> ` :
                            ` <p class="product__price">${formattedPrice}</p>`}
                                                </div>
                                            </div>
                                             ${isNewArrival ? `<div class="new-arrival-badge">New Arrival</div>` : ''}
                                             ${isSale ? `<div class="sale-badge">-${product.productSale}%</div>` : ''}
                                        </div>
                                    </div>
                                </div>
                            `;
                    $('#product-list').append(productHTML);
                });
            } else {
                alert('Không có sản phẩm nào hoặc dữ liệu không hợp lệ.');
            }
        },
        error: function () {
            alert('Đã xảy ra lỗi khi tải sản phẩm.');
        }
    });
}



function toggleFilter() {
    var filterSlide = document.getElementById('filter-slide');
    var overlay = document.getElementById('overlay_filter');


    filterSlide.classList.toggle('open');


    overlay.style.display = filterSlide.classList.contains('open') ? 'block' : 'none';
}

function closeFilter() {
    var filterSlide = document.getElementById('filter-slide');
    var overlay = document.getElementById('overlay_filter');


    filterSlide.classList.remove('open');

    overlay.style.display = 'none';
}

