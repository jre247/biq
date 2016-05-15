####EF migrations will cause bad things in BL-OLTP for BL-OLAP projects
#####Must be done every time a data migration is run.

1.  Run all migrations on localhost.BL-OLTP.
1.  Delete the BrightLine.Data.OLTP_<date>_<time> database reference in BrightLine.Data.OLTP.
1.  Delete the BrightLine.Data.OLTP_<date>_<time> database reference in BrightLine.Data.OLAP.
1.  Exclude all files from BL-OLTP project. (Easier before update than after)
1.  Right click the BL-OLTP project, select 'Schema Compare'.
1.  Select the BrightLine.Data.BL-OLTP project as the target.
1.  Select localhost.BL-OLTP as the source.
1.  Tick checkboxes for all EF managed resources.
1.  Right click BL-OLTP project, select 'Snapshot Project'.
1.  Remove all EF managed resources in BL-OLTP.
1.  Include all unmanaged resources in BL-OLTP.
1.  In BL-OLAP, select Add Database Reference, Data-Tier Application, Browse to snapshot just created, Database location: same database, database name: BL-OLTP,  clear Database- variable, select Suppress errors in referenced project. Click Ok.
1.  In BL-OLAP, select Add Database Reference, Data-Tier Application, Browse to snapshot just created, Database location: different database, same server, database name: BL-OLTP,  clear Database- variable, select Suppress errors in referenced project. Click Ok.

######And you're done.