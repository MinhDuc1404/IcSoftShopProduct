function changePage(pageNumber) {
    // Ngừng sự kiện mặc định của thẻ <a>
    event.preventDefault();

    $.ajax({
        url: '@Url.Page("/ManageProduct/Index")',
        type: 'GET',
        data: { pageNumber: pageNumber },
        success: function (response) {
            // Cập nhật danh sách sản phẩm
            $('.product-list-container').html(renderProductList(response.products, response.domainUrl));

            // Cập nhật phân trang
            $('.pagination').html(renderPagination(response.totalPages, response.currentPage));
        }
    });
}

function deleteProduct(productId) {
    if (confirm('Bạn có chắc muốn xóa sản phẩm này không?')) {
        $.ajax({
            url: '/ManageProduct/Index?handler=Delete&id=' + productId,
            type: 'GET',
            data: { id: productId },
            success: function (response) {
                if (response.success) {
                    alert('Xóa sản phẩm thành công');
                    changePage(1); // Reload lại trang sau khi xóa
                } else {
                    alert('Xảy ra lỗi khi xóa sản phẩm');
                }
            },
            error: function (error) {
                alert('Lỗi khi xóa sản phẩm');
            }
        });
    }
}

// Hàm render lại danh sách sản phẩm
function renderProductList(products, domainUrl) {
    var productHtml = '';
    products.forEach(function (product) {
        productHtml += '<tr>';
        productHtml += '<td data-label="Hình ảnh">';
        productHtml += '<img src="' + domainUrl + (product.productImage[0]?.imageUrl || '') + '" alt="Ảnh sản phẩm" />';
        productHtml += '</td>';
        productHtml += '<td data-label="Tên sản phẩm">' + product.productName + '</td>';
        productHtml += '<td data-label="Giá sản phẩm">' + product.productPrice.toLocaleString() + ' ₫</td>';
        productHtml += '<td data-label="Kho">Còn ' + product.productQuantity + '</td>';
        productHtml += '<td data-label="Thao tác">';
        productHtml += '<a href="/ManageProduct/Update/' + product.productId + '" class="btn btn-warning btn-sm">Sửa</a>';
        productHtml += '<button type="button" class="btn btn-danger btn-sm" onclick="deleteProduct(' + product.productId + ')">Xóa</button>';
        productHtml += '</td>';
        productHtml += '</tr>';
    });
    return productHtml;
}

// Hàm render lại phân trang
function renderPagination(totalPages, currentPage) {
    var paginationHtml = '';
    if (currentPage > 1) {
        paginationHtml += '<a href="#" class="btn btn-secondary" onclick="changePage(' + (currentPage - 1) + ')">Trang trước</a>';
    }

    for (var i = 1; i <= totalPages; i++) {
        paginationHtml += '<a href="#" class="btn ' + (i === currentPage ? 'btn-primary' : 'btn-light') + '" onclick="changePage(' + i + ')">' + i + '</a>';
    }

    if (currentPage < totalPages) {
        paginationHtml += '<a href="#" class="btn btn-secondary" onclick="changePage(' + (currentPage + 1) + ')">Trang sau</a>';
    }

    return paginationHtml;


}