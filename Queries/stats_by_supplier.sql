Create View view2 AS
select name as 'Supplier', cname as 'City', sum(amount_payable) as 'Amount_Payable',
sum(amount_paid) as 'Amount_Paid', sum(amount_paid) - sum(amount_payable)
as 'Balance', sum(discount) as 'Discount'
from transactionvoucher, vouch_shop, shop, shop_city
where
vouch_shop.voucher_numb = transactionvoucher.voucher_numb
and vouch_shop.shop_no = shop.shop_no and shop_city.cnumb = shop.cnumb
group by name, cname;
