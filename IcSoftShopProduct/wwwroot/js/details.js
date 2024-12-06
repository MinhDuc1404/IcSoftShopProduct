document.addEventListener("DOMContentLoaded", function () {
    const productSlider = document.querySelector('.product__slider'); 
    let isMobile = window.innerWidth <= 1024; 

    if (isMobile) {
        let isDown = false;
        let startX;
        let scrollLeft;

        productSlider.addEventListener('mousedown', (e) => {
            isDown = true;
            productSlider.classList.add('active');
            startX = e.pageX - productSlider.offsetLeft;
            scrollLeft = productSlider.scrollLeft;
        });

        productSlider.addEventListener('mouseleave', () => {
            isDown = false;
            productSlider.classList.remove('active');
        });

        productSlider.addEventListener('mouseup', () => {
            isDown = false;
            productSlider.classList.remove('active');
        });

        productSlider.addEventListener('mousemove', (e) => {
            if (!isDown) return;
            e.preventDefault();
            const x = e.pageX - productSlider.offsetLeft;
            const walk = (x - startX) * 3; // Điều chỉnh tốc độ kéo
            productSlider.scrollLeft = scrollLeft - walk;
        });
    }
});




window.addEventListener('scroll', function () {
 
    if (window.innerWidth > 1024) {
        var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        var col6 = document.querySelector('.col-lg-6');
        var col1 = document.querySelector('.col-lg-1');
        var col6MaxScroll = col6.scrollHeight - col6.clientHeight;

        // Tính toán vị trí cuộn của trang
        var documentMaxScroll = document.documentElement.scrollHeight - window.innerHeight;

        var speedFactor = 4;  

        // Tính toán cuộn của col-lg-6 nhanh hơn
        var col6ScrollPos = (scrollTop / documentMaxScroll) * col6MaxScroll * speedFactor;
        col6.scrollTop = col6ScrollPos;
    }
});



document.querySelector('.toggle-description').addEventListener('click', function () {
    const icon = this.querySelector('.toggle-description-icon');
    const border = document.querySelector
    const content = document.getElementById('descriptionContent');

    icon.classList.toggle('rotate');

    if (content.style.display === "none" || content.style.display === "") {
        content.style.display = "block";
        border.style.border = "1px solid black";
    } else {
        content.style.display = "none";
    }
});


document.querySelector('.toggle-shipping').addEventListener('click', function () {
    const icon = this.querySelector('.toggle-shipping-icon');
    const content = document.getElementById('shippingContent');

    icon.classList.toggle('rotate');

    if (content.style.display === "none" || content.style.display === "") {
        content.style.display = "block";
    } else {
        content.style.display = "none";
    }
});


document.querySelector('.toggle-exchange').addEventListener('click', function () {
    const icon = this.querySelector('.toggle-exchange-icon');
    const content = document.getElementById('exchangeContent');

    icon.classList.toggle('rotate');

    if (content.style.display === "none" || content.style.display === "") {
        content.style.display = "block";
    } else {
        content.style.display = "none";
    }
});




// Hàm để giảm số lượng
function decreaseQty() {
    var qtyInput = document.getElementById('qtym');
    var stickResult = document.getElementsByClassName('pd-qtym')[1];
    var qty = parseInt(qtyInput.value);

    if (!isNaN(qty) && qty > 1) {
        qtyInput.value = qty - 1;

    }
}

// Hàm để tăng số lượng
function increaseQty() {
    var qtyInput = document.getElementById('qtym');
    var stickResult = document.getElementsByClassName('pd-qtym')[1];
    var qty = parseInt(qtyInput.value);

    if (!isNaN(qty)) {
        qtyInput.value = qty + 1;

    }
}

// Kiểm tra xem người dùng nhập có phải là số hay không
function isNumber(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false; 
    }
    return true;
}

// Đồng bộ số lượng khi người dùng thay đổi trực tiếp giá trị
function syncQuantity() {
    var qtyInput = document.getElementById('qtym');
    var stickResult = document.getElementsByClassName('pd-qtym')[1];

    if (qtyInput.value == 0) {
        qtyInput.value = 1; 
    }
    stickResult.value = qtyInput.value;
}


let currentIndex = 0;  
let visibleThumbs = 5; 
const thumbs = document.querySelectorAll('#thumb-list .thumbnail-item');
const totalThumbs = thumbs.length;

document.addEventListener("DOMContentLoaded", function () {
    var mainImage = document.getElementById('main-image');
    $('.thumbnail-item').first().addClass('active');

    // Hàm cập nhật ảnh lớn khi click vào thumbnail
    document.querySelectorAll('.nav-link').forEach(function (thumb, index) {
        thumb.addEventListener('click', function () {
       
            currentIndex = index;  
            var targetImage = this.getAttribute('data-target');
            updateMainImage(targetImage);
            ensureVisible(currentIndex);  
            updateActiveThumbnail(currentIndex); 
        });
    });

    // Nút Previous
    document.getElementById('prev-btn').addEventListener('click', function () {
        if (currentIndex > 0) {
            currentIndex--;  
            var targetImage = thumbs[currentIndex].querySelector('a').getAttribute('data-target');
            updateMainImage(targetImage);
            ensureVisible(currentIndex);  
            updateActiveThumbnail(currentIndex);
        }
    });

    // Nút Next
    document.getElementById('next-btn').addEventListener('click', function () {
        if (currentIndex < totalThumbs - 1) {
            currentIndex++;  
            var targetImage = thumbs[currentIndex].querySelector('a').getAttribute('data-target');
            updateMainImage(targetImage);
            ensureVisible(currentIndex);  
            updateActiveThumbnail(currentIndex);
        }
    });

    // Hàm để cập nhật ảnh lớn
    function updateMainImage(targetImage) {
        $(mainImage).fadeOut(300, function () {
            $(this).attr('src', targetImage).fadeIn(300);
        });
    }

    // Hàm để đảm bảo ảnh con hiện tại nằm trong vùng hiển thị
    function ensureVisible(index) {

        let start = Math.max(0, index - Math.floor(visibleThumbs / 2));
        let end = Math.min(totalThumbs, start + visibleThumbs);

        thumbs.forEach((thumb, i) => {
            if (i >= start && i < end) {
                thumb.style.display = 'block';  
            } else {
                thumb.style.display = 'none';  
            }
        });
    }

    // Cập nhật lớp active cho thumbnail
    function updateActiveThumbnail(index) {
        $('.thumbnail-item').removeClass('active');
        $('.thumbnail-item').eq(index).addClass('active');
    }
});
document.querySelector('.add-to-cart-btn').addEventListener('click', function () {
    this.classList.add('active');
    setTimeout(() => {
        this.classList.remove('active');
    }, 200); // Thời gian delay 200ms
});




document.querySelector('.close-btn').addEventListener('click', function () {
    document.getElementById('cart-notification-modal').style.display = 'none';
});


