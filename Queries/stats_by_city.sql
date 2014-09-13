select cname as 'Supplier', sum(amount_payable) as 'Amount Payable',
sum(amount_paid) as 'Amount Paid', sum(amount_paid) - sum(amount_payable)
as 'Balance'
from transactionvoucher, vouch_shop, shop, shop_city
where
vouch_shop.voucher_numb = transactionvoucher.voucher_numb
and vouch_shop.shop_no = shop.shop_no and shop.cnumb = shop_city.cnumb
group by cname;