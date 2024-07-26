/***
 * 
                Favourites
 * 
 */

CheckFave();

function CheckFave() {
    try {
        let faveNo = document.getElementById("floorWishlistID");
        let faveNo2 = document.getElementById("topWishlistID1");
        let faveNo3 = document.getElementById("topWishlistID2");

        const getFaveData = localStorage.getItem('fave');
        let fave = [];


        if (getFaveData != null) {
            fave = JSON.parse(getFaveData);

            try {
                var faveCount = fave.length;
                faveNo.innerText = faveCount;
                faveNo2.innerText = faveCount;
                faveNo3.innerText = faveCount;
            }
            catch (e) {

            }
        }
    }
    catch { }
}
function ClearFave() {
    var fave = [];
    const jsonData = JSON.stringify(fave);
    localStorage.setItem('fave', jsonData);

    location.reload();
    
    alertDanger("Watchlist cleared!");
}
function ToggleFave(itemID) {
    const retrievedFave = localStorage.getItem('fave');
    if (retrievedFave == null) {
        fave = [];
        fave.push(itemID);
        const jsonData = JSON.stringify(fave);
        localStorage.setItem('fave', jsonData);
    } else {
        fave = JSON.parse(retrievedFave);

        const index = fave.includes(itemID);
        //alert(index)

        if (index) {
            const index = fave.indexOf(itemID);
            fave.splice(index,1);
            alertDanger("Removed from Watchlist");
        } else {
            alertSuccess("Added to Watchlist");
            fave.push(itemID);
        }

        const jsonData = JSON.stringify(fave);
        localStorage.setItem('fave', jsonData);

    }
    CheckFave();
}

/**
 * 
 *              CART
 */
loadCartData();
function loadCartData() {

    let cartQtyNo = document.getElementById("floorCartID");
    let cartQtyNo2 = document.getElementById("topCart1");
    let cartQtyNo3 = document.getElementById("topCart2");
    try {


        const getCartData = localStorage.getItem('cart');
        let cart = [];


        if (getCartData != null) {
            cart = JSON.parse(getCartData);

            try {
                var count = cart.length;
                cartQtyNo.innerText = count;
                cartQtyNo2.innerText = count;
                cartQtyNo3.innerText = count;
            }
            catch (e) {

            }
        }
    }
    catch { }
}

function AddFromItem(itemID, qtyValID) {
    let qtyVal = document.getElementById(qtyValID);

    AddToCart(itemID, qtyVal.value);
}

function AddToCart(itemID, qtyVal, feature, qtyID) {
    let itemQty;
    let quantity;

    try {
        itemQty = document.getElementById(qtyID);
        quantity = itemQty.value;
    } catch (e) {
        quantity = qtyVal;
    }

    if (quantity < 1)
        alertDanger("Quantity less than 1");

    var data = {};
    if (feature == undefined) {
       data = { ID: itemID, Qty: quantity }
    } else {
        data = { ID: itemID, Qty: quantity, Feature: feature }
    }
    //console.log(data, "Final Cart Value")

    const retrievedData = localStorage.getItem('cart');

    if (retrievedData == null) {
        cart = [];
        cart.push(data);
        const jsonData = JSON.stringify(cart);
        localStorage.setItem('cart', jsonData);
    } else {
        cart = JSON.parse(retrievedData);

        const index = cart.findIndex(o => o.hasOwnProperty("ID") && o["ID"] === data.ID);
        //alert(index)

        //if (index !== -1) {
        //    cart[index].Qty = data.Qty;
        //} else {
        //    cart.push(data);
        //}
        cart.push(data);

        const jsonData = JSON.stringify(cart);
        localStorage.setItem('cart', jsonData);
       // console.log(cart)
    }
    alertSuccess("Added to cart");
    try {
        //cartQtyNo = document.getElementById("cartQtyNo");
        cartQtyNo.innerText = cart.length;
        cartQtyNo2.innerText = cart.length;
    }
    catch (e) {

    }

    loadCartData();
}

function UpdateCartQty(index, inputID, costID, price) {
    input = document.getElementById(inputID);
    costLabel = document.getElementById(costID);

    cart[index].Qty = input.value;
    costLabel.innerText = price * cart[index].Qty;

    const jsonData = JSON.stringify(cart);
    localStorage.setItem('cart', jsonData);

    loadCartData();
    alertSuccess("Cart updated");
}

//function ClearCart() {
   

//    var cart = [];
//    const jsonData = JSON.stringify(cart);
//    localStorage.setItem('cart', jsonData);

   


//    loadCartData();
//    alertDanger("Cart cleared!");
//}
function ClearCart() {

    var cart = [];
    const jsonData = JSON.stringify(cart);
    localStorage.setItem('cart', jsonData);

    var cartHolder = document.getElementById("cartHolder");
    if (cartHolder != null) {

        cartHolder.innerHTML = "";
        cartHolder.innerHTML = '<div class="container-fluid"><div class="row px-xl-5">  <div class="col-12"> <div> <h5>No Content Found!</h5></div> </div> </div></div>';
    }

    loadCartData();
    alertDanger("Cart cleared!");
}


function alertSuccess(val) {
    alertThis(val, "success");
}
function alertDanger(val) {
    alertThis(val, "danger");
}
function alertWarning(val) {
    alertThis(val, "warning");
}

function alertThis(val, type) {
    var div = document.createElement("div");
    div.innerText = val;
    div.classList.add("slide-in-left");
    div.classList.add("alertFloater");

    try {
        div.classList.remove("alertSuccess");
        div.classList.remove("alertWarning");
        div.classList.remove("alertDanger");
        div.classList.remove("hidden");
    } catch (e) {

    }

    if (type == "success") {
        div.classList.add("alertSuccess");
    }
    else if (type == "warning") {
        div.classList.add("alertWarning");
    }
    else if (type == "danger") {
        div.classList.add("alertDanger");
    }


    document.getElementById("alertMe").append(div)

    setTimeout(() => {
        div.classList.add("hidden");
    }, 3000)
}


function IsImageFile(fileInput) {
    try {
        fileName = fileInput.files[0].name;
        var imageExtensions = ['jpg', 'jpeg', 'png']; // Add more extensions if needed
        var extension = fileName.split('.').pop().toLowerCase();
        return imageExtensions.includes(extension);
    } catch (e) {
        return false;
    }
}

function Pay() {
    let OTPForm = document.getElementById("OTPForm");
    let cardForm = document.getElementById("cardForm");
    let cardNo = document.getElementById("cardNo");
    let expiry = document.getElementById("expiry");
    let cvv = document.getElementById("cvv");
    let amount = document.getElementById("amount");
    let pin = document.getElementById("pin");
    let loaderSpace = document.getElementById("loaderSpace");
    let rechargeBtn = document.getElementById("rechargeBtn");
    let statMessage = document.getElementById("statMessage");

    if (cardNo.value == "" || expiry.value == "" || cvv.value == "" || amount.value == "" || pin.value == "") {
        alertDanger("Invalid Details")
        statMessage.innerText = "Invalid Details";
        return;
    }

    loaderSpace.classList.remove("hidden");
    rechargeBtn.classList.add("hidden");
    statMessage.innerText = "";

    dataObj = {
        amount: amount.value,
        cardNo: cardNo.value,
        cvv: cvv.value,
        expiry: expiry.value,
        pin: pin.value
    };


    try {
        $.ajax({
            type: "POST",
            url: '/Dashboard/Recharge',
            data: JSON.stringify(dataObj),
            contentType: "application/json;",
            success: function (response) {
                cardForm.classList.add("hidden");
                OTPForm.classList.remove("hidden");
            },
            error: function (response) {
                alertDanger(response.responseText);
                loaderSpace.classList.add("hidden");
                rechargeBtn.classList.remove("hidden");
                statMessage.innerText = response.responseText;
            }
        });

    } catch (e) {

    }
}


function OTP() {
    let otp = document.getElementById("otp");
    let otpLoader = document.getElementById("otpLoader");
    let otpBtn = document.getElementById("otpBtn");
    let statMessage = document.getElementById("statMessage");

    if (otp.value == "") {
        alertDanger("Invalid OTP")
        statMessage.innerText = "Invalid OTP";
        return;
    }

    otpLoader.classList.remove("hidden");
    otpBtn.classList.add("hidden");
    statMessage.innerText = "";

    try {
        $.ajax({
            type: "POST",
            url: '/Dashboard/OTP',
            data: JSON.stringify(otp.value),
            contentType: "application/json;",
            success: function (response) {
                alertSuccess("Payment Successful");
                location.reload();
            },
            error: function (response) {
                alertDanger(response.responseText);
                otpBtn.classList.remove("hidden");
                otpLoader.classList.add("hidden");
                statMessage.innerText = response.responseText;
            }
        });

    } catch (e) {

    }
}

function SendReview(itemID, message, name, email, rating) {
    
    var review = {
        itemID: itemID,
        message: message,
        name: name,
        email: email,
        rating: rating
    }
    var reviewHolder = document.getElementById("reviewHolder");
    reviewHolder.innerHTML = '<div class="text-center"><img src="../gif/loader.gif" style="width:100px;height:100px;" /></div>';
    try {
        $.ajax({
            type: "POST",
            url: '/shop/addReview',
            data: JSON.stringify(review),
            contentType: "application/json;",
            success: function (response) {
                reviewHolder.innerHTML = response;
            },
            error: function (response) {
                location.reload()
            }
        });

    } catch (e) {

    }
}

function CopyToClipboard(text, alert) {
    if (navigator.clipboard) {
        navigator.clipboard.writeText(text)
            .then(() => {
                alertSuccess(alert)
            })
            .catch(err => {
                console.error('Could not copy text: ', err);
            });
    }
}



//function AccessPop() {
//    let blur = document.createElement("div");
//    let holder = document.createElement("div");
//    let btnHolder = document.createElement("div");
//    let loaderHolder = document.createElement("div");
//    let small = document.createElement("small");
//    let headline = document.createElement("h3");
//    let meta = document.createElement("p");
//    let img = document.createElement("img");
//    let loader = document.createElement("img");
//    let close = document.createElement("span");

//    let amountInput = document.createElement("input");

//    let send = document.createElement("button");
//    let cancel = document.createElement("button");


//    send.innerText = "Send"
//    blur.classList.add("blur");
//    send.classList.add("btn");
//    send.classList.add("btn-success");
//    send.classList.add("btn-sm");
//    send.classList.add("full");
//    holder.classList.add("giftPop");
//    loader.classList.add("popSendLoader");
//    small.classList.add("popSmall");

//    headline.innerText = "Gift CampusCoin (CC)";
//    close.innerText = "x";
//    img.src = "../gif/treasure.gif"
//    loader.src = "../gif/loader.gif"
//    amountInput.classList.add("form-control")
//    amountInput.placeholder = "Amount of CC to send"
//    amountInput.type = "number"
//    amountInput.min = 1;
//    amountInput.id = "popGiftInput"
//    blur.id = "blur"
//    small.innerText = "Appreciate the creator of this resources with some CampusCoin for first time access."


//    btnHolder.appendChild(headline)
//    btnHolder.appendChild(amountInput)
//    btnHolder.appendChild(send)
//    loaderHolder.appendChild(loader)
//    loaderHolder.classList.add("vertical-center");

//    holder.appendChild(close)
//    holder.appendChild(img)
//    holder.appendChild(btnHolder);
//    holder.appendChild(small);

//    close.addEventListener("click", () => {
//        //confirmTicketToggle = false;
//        //form.submit();

//        blur.parentNode.removeChild(blur);
//    });

//    send.addEventListener("click", () => {
//        //confirmTicketToggle = false;
//        //form.submit();
//        let popGiftInput = document.getElementById("popGiftInput");
//        if (popGiftInput.value == "" || popGiftInput.value < 1) {
//            alertDanger("Input valid amount");
//        } else {
//            btnHolder.classList.add("hidden");
//            holder.appendChild(loaderHolder);

//           /* SendAccessGift(popGiftInput.value, context, contextID, receiverID, message, contID);*/
//        }

//    });

//    blur.appendChild(holder);

//    document.getElementById("alertMe").append(blur)
//}

function CheckStore(storeLevel, requiredLevel) {
    if (storeLevel >= requiredLevel) {
        true;
    }
    alertDanger("Upgrade Required!")
    return false;
}

function ResolveQuantity(isAdd, quantityID, itemPrice, itemTotalID, itemID) {

    var qtyInput = document.getElementById(quantityID);
    var itemTotal = document.getElementById(itemTotalID);
    var subTotal = document.getElementById("subTotal");
    var shipping = document.getElementById("shipping");
    var total = document.getElementById("total");
    
    var subT = subTotal.innerText * 1;
    var shippn = shipping.innerText * 1;
    var itmTotal = itemTotal.innerText * 1;

    subT = subT - itmTotal;


    var val = qtyInput.value * 1;
    var price = itemPrice * 1;

    if (isAdd == "add") {
        val++;
    } else if (isAdd == "minus") {
        val--;
    }
    val = val < 1 ? 1 : val;

    qtyInput.value = val;
    itmTotal = val * price;
    itemTotal.innerText = itmTotal;

    subT += itmTotal;
    var totl = subT + shippn;
    subTotal.innerText = subT;
    total.innerText = totl;


    const retrievedData = localStorage.getItem('cart');
    if (retrievedData != null) {
        var cart = JSON.parse(retrievedData);
        const index = cart.findIndex(o => o.hasOwnProperty("ID") && o["ID"] === itemID);
        cart[index].Qty = val;

        const jsonData = JSON.stringify(cart);
        localStorage.setItem('cart', jsonData);
    }
    
}


function RemoveFromCart(ID, tableIndex, totalLabel) {
    var cartTable = document.getElementById("cartTable");
    const retrievedData = localStorage.getItem('cart');

    if (retrievedData != null) {

        var itemTotal = document.getElementById(totalLabel);
        var total = document.getElementById("total");
        var subTotal = document.getElementById("subTotal");
        var shipping = document.getElementById("shipping");

        var subT = subTotal.innerText * 1;
        var itmTotal = itemTotal.innerText * 1;
        var shippn = shipping.innerText * 1;

        subT -= itmTotal;
        subTotal.innerText = subT;
        total.innerText = subT + shippn;

        var cart = JSON.parse(retrievedData);
        const index = cart.findIndex(o => o.hasOwnProperty("ID") && o["ID"] === ID);

        cartTable.deleteRow(index+1);

        
        cart.splice(index, 1);

        const jsonData = JSON.stringify(cart);
        localStorage.setItem('cart', jsonData);

        loadCartData();

      
        //console.log(cartTable.rows.length);
        //console.log(tableIndex);
       
    }
}


function GenerateLink(message, number) {
    //let number = "+2348137395582";
    let url = "https://wa.me/";
    let end_url = `${url}${number}?text=${message}`;

    window.open(end_url, '_blank');

}
function QuickView(itemTag) {

    try {
        var holder = document.getElementById("quickViewID");
        holder.innerHTML = '<div class="text-center"><img src="../gif/loader.gif" style="width:100px;height:100px;" /></div>';
        $.ajax({
            type: "POST",
            url: '/shop/quickview',
            data: JSON.stringify(itemTag),
            contentType: "application/json;",
            success: function (response) {
                holder.innerHTML = response;
            },
            error: function (response) {
                var close = document.getElementById("closeBtn");
                close.click();
                alertDanger("Item Not Found!");
            }
        });

    } catch (e) {

    }
}
function GetFullCart() {
    const getCartData = localStorage.getItem('cart');
    $.ajax({
        type: "POST",
        url: '/shop/getcart',
        data: JSON.stringify(getCartData),
        contentType: "application/json;",
        success: function (response) {
            var cartHolder = document.getElementById("cartHolder");
            cartHolder.innerHTML = response;
        },
        error: function (response) {
            /*  alert("bad");*/
        }

    });
}
function QuickCart() {

    try {
        const getCartData = localStorage.getItem('cart');
        var holder = document.getElementById("navCartID");
        holder.innerHTML = '<div class="text-center"><img src="../gif/loader.gif" style="width:50px;height:50px;" /></div>';
        $.ajax({
            type: "POST",
            url: '/shop/quickcart',
            data: JSON.stringify(getCartData),
            contentType: "application/json;",
            success: function (response) {
                holder.innerHTML = response;
            },
            error: function (response) {
                var close = document.getElementById("closeBtn");
                close.click();
                alertDanger("Items Not Found!");
            }
        });

    } catch (e) {

    }
}