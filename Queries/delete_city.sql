select * from dbo.shop_city
delete from dbo.shop_city where cnumb > 6

delete from transactionvoucher where
transactionvoucher.voucher_numb = vouch_shop.voucher_numb
and vouch_shop.shop_no = 4