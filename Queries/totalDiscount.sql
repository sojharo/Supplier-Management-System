select sum(discount) as "Total Discount"
from transactionvoucher, vouch_shop 
where transactionvoucher.voucher_numb = vouch_shop.voucher_numb 
and vouch_shop.shop_no = 2