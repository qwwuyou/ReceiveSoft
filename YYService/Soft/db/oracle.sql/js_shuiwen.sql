-----------------------------------------------------
-- Export file for user SYSTEM@JS_SHUIWEN          --
-- Created by Administrator on 2016/8/17, 15:59:30 --
-----------------------------------------------------

set define off
spool js_shuiwen.log

prompt
prompt Creating table CENTER_RTUCHANGE
prompt ===============================
prompt
create table SYSTEM.CENTER_RTUCHANGE
(
  projectname NVARCHAR2(50) not null,
  publicip    NVARCHAR2(20) not null,
  rtucount    NUMBER,
  dtime       DATE not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.CENTER_RTUCHANGE
  add primary key (PROJECTNAME, PUBLICIP)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CENTER_SERVER
prompt ============================
prompt
create table SYSTEM.CENTER_SERVER
(
  projectname  NVARCHAR2(50) not null,
  publicip     NVARCHAR2(20) not null,
  runtime      NVARCHAR2(50),
  registertime NUMBER,
  rtucount     NUMBER,
  dtime        DATE not null,
  runstate     NVARCHAR2(10),
  srarttime    DATE
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.CENTER_SERVER
  add primary key (PROJECTNAME, PUBLICIP)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table CENTER_STARTSTATE
prompt ================================
prompt
create table SYSTEM.CENTER_STARTSTATE
(
  projectname NVARCHAR2(50) not null,
  publicip    NVARCHAR2(20) not null,
  runtime     NVARCHAR2(50),
  dtime       DATE not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.CENTER_STARTSTATE
  add primary key (PROJECTNAME, PUBLICIP, DTIME)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_COMMAND_TEMP
prompt ==============================
prompt
create table SYSTEM.YY_COMMAND_TEMP
(
  stcd      NVARCHAR2(20) not null,
  nfoindex  NUMBER not null,
  commandid NVARCHAR2(40) not null,
  data      LONG not null,
  tm        DATE not null,
  state     NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_COMMAND_TEMP
  add primary key (COMMANDID, STCD, NFOINDEX)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_DATA_AUTO
prompt ===========================
prompt
create table SYSTEM.YY_DATA_AUTO
(
  stcd            NVARCHAR2(20) not null,
  itemid          NVARCHAR2(40) not null,
  tm              DATE not null,
  downdate        DATE,
  nfoindex        NUMBER,
  datavalue       NUMBER(20,10),
  correctionvalue NUMBER(20,10),
  datatype        NUMBER,
  sttype          NVARCHAR2(10)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_DATA_AUTO
  add primary key (ITEMID, STCD, TM)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_DATA_COMMAND
prompt ==============================
prompt
create table SYSTEM.YY_DATA_COMMAND
(
  commandid NVARCHAR2(40) not null,
  stcd      NVARCHAR2(20) not null,
  tm        DATE not null,
  downdate  DATE,
  state     NUMBER,
  nfoindex  NUMBER,
  command   NVARCHAR2(500)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_DATA_COMMAND
  add primary key (COMMANDID, STCD, TM)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_DATA_IMG
prompt ==========================
prompt
create table SYSTEM.YY_DATA_IMG
(
  stcd      NVARCHAR2(20) not null,
  tm        DATE not null,
  downdate  DATE,
  nfoindex  NUMBER,
  datavalue LONG RAW,
  info      NVARCHAR2(2000),
  datatype  NUMBER
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_DATA_IMG
  add primary key (STCD, TM)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_DATA_LOG
prompt ==========================
prompt
create table SYSTEM.YY_DATA_LOG
(
  stcd     NVARCHAR2(20) not null,
  tm       DATE not null,
  logid    NUMBER,
  downdate DATE,
  nfoindex NUMBER,
  count    NUMBER,
  erc      NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_DATA_LOG
  add primary key (STCD, TM, ERC)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_DATA_MANUAL
prompt =============================
prompt
create table SYSTEM.YY_DATA_MANUAL
(
  stcd      NVARCHAR2(20) not null,
  tm        DATE not null,
  downdate  DATE,
  nfoindex  NUMBER,
  datavalue NUMBER(20,10),
  datatype  NUMBER
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_DATA_MANUAL
  add primary key (STCD, TM)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_DATA_REM
prompt ==========================
prompt
create table SYSTEM.YY_DATA_REM
(
  stcd      NVARCHAR2(20) not null,
  itemid    VARCHAR2(40) not null,
  tm        DATE not null,
  downdate  DATE,
  nfoindex  NUMBER,
  datavalue NUMBER(20,10),
  datatype  NUMBER
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_DATA_REM
  add primary key (STCD, ITEMID, TM)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_DATA_STATE
prompt ============================
prompt
create table SYSTEM.YY_DATA_STATE
(
  stcd      NVARCHAR2(20) not null,
  tm        DATE not null,
  downdate  DATE,
  nfoindex  NUMBER,
  statedata NVARCHAR2(40),
  datavalue NUMBER(20,1)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_DATA_STATE
  add primary key (STCD, TM)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_LOG
prompt =====================
prompt
create table SYSTEM.YY_LOG
(
  logid  NUMBER not null,
  rtulog NVARCHAR2(200)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_LOG
  add primary key (LOGID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_BASIC
prompt ===========================
prompt
create table SYSTEM.YY_RTU_BASIC
(
  stcd     NVARCHAR2(20) not null,
  password NVARCHAR2(6),
  nicename NVARCHAR2(50)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_RTU_BASIC
  add primary key (STCD)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_BI
prompt ========================
prompt
create table SYSTEM.YY_RTU_BI
(
  stcd   NVARCHAR2(20) not null,
  itemid NVARCHAR2(40) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_RTU_BI
  add primary key (STCD, ITEMID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_COMMAND
prompt =============================
prompt
create table SYSTEM.YY_RTU_COMMAND
(
  commandid NVARCHAR2(40) not null,
  remark    NVARCHAR2(500),
  state     NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_RTU_COMMAND
  add primary key (COMMANDID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_CONFIGDATA
prompt ================================
prompt
create table SYSTEM.YY_RTU_CONFIGDATA
(
  stcd      NVARCHAR2(10) not null,
  itemid    NVARCHAR2(40) not null,
  configid  NVARCHAR2(40) not null,
  configval NVARCHAR2(100)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_RTU_CONFIGDATA
  add primary key (STCD, ITEMID, CONFIGID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_CONFIGITEM
prompt ================================
prompt
create table SYSTEM.YY_RTU_CONFIGITEM
(
  configid   NVARCHAR2(40) not null,
  configitem NVARCHAR2(20)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_RTU_CONFIGITEM
  add primary key (CONFIGID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_ITEM
prompt ==========================
prompt
create table SYSTEM.YY_RTU_ITEM
(
  itemid      NVARCHAR2(40) not null,
  itemname    NVARCHAR2(50) not null,
  itemcode    NVARCHAR2(10) not null,
  iteminteger NUMBER not null,
  itemdecimal NUMBER not null,
  plusorminus NUMBER not null,
  units       NVARCHAR2(20)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_RTU_ITEM
  add primary key (ITEMID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_ITEMCONFIG
prompt ================================
prompt
create table SYSTEM.YY_RTU_ITEMCONFIG
(
  itemid   NVARCHAR2(40) not null,
  configid NVARCHAR2(40) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_WATERYIELD
prompt ================================
prompt
create table SYSTEM.YY_RTU_WATERYIELD
(
  stcd    NVARCHAR2(20) not null,
  itemid  NVARCHAR2(40) not null,
  stream  NUMBER(10) not null,
  str_pad NUMBER(8),
  tm      DATE,
  type    NUMBER
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_RTU_WATERYIELD
  add primary key (STCD, ITEMID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_RTU_WRES
prompt ==========================
prompt
create table SYSTEM.YY_RTU_WRES
(
  stcd         NVARCHAR2(20) not null,
  code         NUMBER not null,
  adr_zx       NUMBER,
  com_m        NUMBER,
  adr_m        NVARCHAR2(20),
  port_m       NUMBER,
  com_b        NUMBER,
  adr_b        NVARCHAR2(20),
  port_b       NUMBER,
  phonenum     NVARCHAR2(15),
  satellitenum NVARCHAR2(15)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_RTU_WRES
  add primary key (STCD, CODE)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt
prompt Creating table YY_STATE
prompt =======================
prompt
create table SYSTEM.YY_STATE
(
  stateid  NVARCHAR2(40) not null,
  rtustate NVARCHAR2(200)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table SYSTEM.YY_STATE
  add primary key (STATEID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );


spool off
