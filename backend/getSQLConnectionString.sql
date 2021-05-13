select
    'data source=' + @@servername +
    ';initial catalog=' + db_name() +
    case type_desc
        when 'WINDOWS_LOGIN' 
            then ';trusted_connection=true'
        else
            ';user id=cms_test;password=cms_password'
    end
    as ConnectionString
from sys.server_principals
where name = suser_name()