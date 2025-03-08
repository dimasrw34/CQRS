CREATE TABLE users (
                       id uuid NOT NULL,
                       email varchar(50) NOT NULL,
                       "password" varchar(128) NOT NULL,
                       firstname varchar(50) NULL,
                       lastname varchar(50) NULL,
                       phone varchar(15) NULL,
                       CONSTRAINT users_pkey PRIMARY KEY (id)
);


CREATE TABLE eventstores (
                             id uuid NOT NULL,
                             datastamp varchar NULL,
                             messagetype varchar NULL,
                             aggregateid uuid NULL,
                             createdat timestamp NULL,
                             CONSTRAINT "EventStores_pk" PRIMARY KEY (id)
);
