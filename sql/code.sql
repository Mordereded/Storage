--
-- PostgreSQL database dump
--
-- Dumped from database version 15.2
-- Dumped by pg_dump version 15.2
SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;
--
-- Name: public.FIO.check; Type: DOMAIN; Schema: public; Owner: postgres
--
CREATE DOMAIN public."public.FIO.check" AS text
	CONSTRAINT fio_check CHECK ((VALUE ~ '^[А-ЯA-Z][А-Яа-яA-Za-z]*$'::text));
ALTER DOMAIN public."public.FIO.check" OWNER TO postgres;
--
-- Name: public.name.check; Type: DOMAIN; Schema: public; Owner: postgres
--
CREATE DOMAIN public."public.name.check" AS text NOT NULL
	CONSTRAINT name_check CHECK ((VALUE ~ '^[А-Яа-яA-Za-zЁё]+$'::text));
ALTER DOMAIN public."public.name.check" OWNER TO postgres;
--
-- Name: create_admin_user(text, text); Type: FUNCTION; Schema: public; Owner: postgres
--
CREATE FUNCTION public.create_admin_user(username text, pass text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Создание нового пользователя
    EXECUTE 'CREATE USER ' || quote_ident(username) || ' WITH PASSWORD ' || quote_literal(pass);
    -- Назначение роли "админ" пользователю
    GRANT "Admin" TO username;
    -- Присвоение прав для выполнения необходимых операций
    GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO username;
    ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL PRIVILEGES ON TABLES TO username;
    -- Добавление пользователю права создания баз данных
    ALTER USER username CREATEDB;
    -- Добавление пользователю права подключения к базе данных
    ALTER USER username CONNECTION LIMIT -1;
END;
$$;
ALTER FUNCTION public.create_admin_user(username text, pass text) OWNER TO postgres;
--
-- Name: create_employee_user(text, text); Type: FUNCTION; Schema: public; Owner: postgres
--
CREATE FUNCTION public.create_employee_user(username text, pass text) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Создание нового пользователя
    EXECUTE 'CREATE USER ' || quote_ident(username) || ' WITH PASSWORD ' || quote_literal(pass);
    -- Назначение роли "работник" пользователю
    GRANT Employee TO username;
    -- Присвоение прав для выполнения необходимых операций
    -- Добавление пользователю права подключения к базе данных
    ALTER USER username CONNECTION LIMIT -1;
END;
$$;
ALTER FUNCTION public.create_employee_user(username text, pass text) OWNER TO postgres;
--
-- Name: get_table_columns(text); Type: FUNCTION; Schema: public; Owner: postgres
--
CREATE FUNCTION public.get_table_columns(tablename text) RETURNS TABLE(column_name information_schema.sql_identifier)
    LANGUAGE plpgsql
    AS $$
DECLARE
  query_string text;
BEGIN
  query_string := 'SELECT column_name::information_schema.sql_identifier FROM information_schema.columns WHERE table_name = ' || quote_literal(tablename);
  RETURN QUERY EXECUTE query_string;
END;
$$;
ALTER FUNCTION public.get_table_columns(tablename text) OWNER TO postgres;
SET default_tablespace = '';
SET default_table_access_method = heap;
--
-- Name: Attribute; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Attribute" (
    id_attribute integer NOT NULL,
    id_category integer NOT NULL,
    attribute_name character varying(255) NOT NULL
);
ALTER TABLE public."Attribute" OWNER TO postgres;
--
-- Name: Attribute_id_attribute_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public."Attribute_id_attribute_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public."Attribute_id_attribute_seq" OWNER TO postgres;
--
-- Name: Attribute_id_attribute_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public."Attribute_id_attribute_seq" OWNED BY public."Attribute".id_attribute;
--
-- Name: Attribute_value; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Attribute_value" (
    id_attribute integer NOT NULL,
    id_attribute_value integer NOT NULL,
    id_item integer NOT NULL,
    attribute_value text NOT NULL
);
ALTER TABLE public."Attribute_value" OWNER TO postgres;
--
-- Name: Attribute_value_id_attribute_value_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public."Attribute_value_id_attribute_value_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public."Attribute_value_id_attribute_value_seq" OWNER TO postgres;
--
-- Name: Attribute_value_id_attribute_value_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public."Attribute_value_id_attribute_value_seq" OWNED BY public."Attribute_value".id_attribute_value;
--
-- Name: Category; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Category" (
    category_name character varying(255) NOT NULL,
    id_category integer NOT NULL,
    CONSTRAINT category_name_check CHECK (((category_name)::text ~ '^[A-Za-zА-Яа-я\s\.,\-]{1,255}$'::text))
);
ALTER TABLE public."Category" OWNER TO postgres;
--
-- Name: Category_id_category_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public."Category_id_category_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public."Category_id_category_seq" OWNER TO postgres;
--
-- Name: Category_id_category_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public."Category_id_category_seq" OWNED BY public."Category".id_category;
--
-- Name: Company; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Company" (
    id_company integer NOT NULL,
    company_name character varying(255) NOT NULL,
    CONSTRAINT company_name_check CHECK (((company_name)::text ~ '^[A-Za-zА-Яа-я\s\.,\-]{1,255}$'::text))
);
ALTER TABLE public."Company" OWNER TO postgres;
--
-- Name: Contract; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Contract" (
    id_contract integer NOT NULL,
    id_customer integer NOT NULL,
    contract_cost integer DEFAULT 0 NOT NULL,
    discription character varying(255),
    id_item integer NOT NULL,
    CONSTRAINT cost_check CHECK ((contract_cost >= 0))
);
ALTER TABLE public."Contract" OWNER TO postgres;
--
-- Name: Contract_id_contract_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public."Contract_id_contract_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public."Contract_id_contract_seq" OWNER TO postgres;
--
-- Name: Contract_id_contract_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public."Contract_id_contract_seq" OWNED BY public."Contract".id_contract;
--
-- Name: Country; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Country" (
    id_country integer NOT NULL,
    country_name character varying(255) NOT NULL,
    CONSTRAINT country_name_check CHECK (((country_name)::text ~ '^[A-Za-zА-Яа-я\s\.,\-]{1,255}$'::text))
);
ALTER TABLE public."Country" OWNER TO postgres;
--
-- Name: Customer; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Customer" (
    id_customer integer NOT NULL,
    first_name public."public.FIO.check" NOT NULL,
    second_name public."public.FIO.check" NOT NULL,
    last_name public."public.FIO.check",
    phone_number character varying(20)
);
ALTER TABLE public."Customer" OWNER TO postgres;
--
-- Name: Item; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Item" (
    id_item integer NOT NULL,
    item_name character varying(255) NOT NULL,
    item_cost money DEFAULT 0 NOT NULL,
    id_category integer NOT NULL,
    id_provider integer NOT NULL,
    item_discription text,
    CONSTRAINT cost_check CHECK ((item_cost >= '0,00 ?'::money))
);
ALTER TABLE public."Item" OWNER TO postgres;
--
-- Name: Item_id_item_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public."Item_id_item_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    MAXVALUE 2147483647
    CACHE 1;
ALTER TABLE public."Item_id_item_seq" OWNER TO postgres;
--
-- Name: Item_id_item_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public."Item_id_item_seq" OWNED BY public."Item".id_item;
--
-- Name: Provider; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public."Provider" (
    first_name character varying(255) NOT NULL,
    second_name character varying(255) NOT NULL,
    last_name character varying(255),
    id_provider integer NOT NULL,
    id_country integer NOT NULL,
    id_company integer NOT NULL,
    CONSTRAINT check_first_name CHECK (((first_name)::text ~ '^[А-ЯA-Z][А-Яа-яA-Za-z]*$'::text)),
    CONSTRAINT check_last_name CHECK (((last_name)::text ~ '^[А-ЯA-Z][А-Яа-яA-Za-z]*$'::text)),
    CONSTRAINT check_second_name CHECK (((second_name)::text ~ '^[А-ЯA-Z][А-Яа-яA-Za-z]*$'::text))
);
ALTER TABLE public."Provider" OWNER TO postgres;
--
-- Name: company_id_company_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public.company_id_company_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public.company_id_company_seq OWNER TO postgres;
--
-- Name: company_id_company_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public.company_id_company_seq OWNED BY public."Company".id_company;
--
-- Name: provider_id_provider_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public.provider_id_provider_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public.provider_id_provider_seq OWNER TO postgres;
--
-- Name: provider_id_provider_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public.provider_id_provider_seq OWNED BY public."Provider".id_provider;
--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--
CREATE TABLE public.users (
    id integer NOT NULL,
    username character varying(255) NOT NULL,
    user_rol character varying(20) NOT NULL
);
ALTER TABLE public.users OWNER TO postgres;
--
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public.users_id_seq OWNER TO postgres;
--
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
--
-- Name: Сustomer_id_customer_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public."Сustomer_id_customer_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public."Сustomer_id_customer_seq" OWNER TO postgres;
--
-- Name: Сustomer_id_customer_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public."Сustomer_id_customer_seq" OWNED BY public."Customer".id_customer;
--
-- Name: сountry_id_country_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--
CREATE SEQUENCE public."сountry_id_country_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
ALTER TABLE public."сountry_id_country_seq" OWNER TO postgres;
--
-- Name: сountry_id_country_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--
ALTER SEQUENCE public."сountry_id_country_seq" OWNED BY public."Country".id_country;
--
-- Name: Attribute id_attribute; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Attribute" ALTER COLUMN id_attribute SET DEFAULT nextval('public."Attribute_id_attribute_seq"'::regclass);
--
-- Name: Attribute_value id_attribute_value; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Attribute_value" ALTER COLUMN id_attribute_value SET DEFAULT nextval('public."Attribute_value_id_attribute_value_seq"'::regclass);
--
-- Name: Category id_category; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Category" ALTER COLUMN id_category SET DEFAULT nextval('public."Category_id_category_seq"'::regclass);
--
-- Name: Company id_company; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Company" ALTER COLUMN id_company SET DEFAULT nextval('public.company_id_company_seq'::regclass);
--
-- Name: Contract id_contract; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Contract" ALTER COLUMN id_contract SET DEFAULT nextval('public."Contract_id_contract_seq"'::regclass);
--
-- Name: Country id_country; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Country" ALTER COLUMN id_country SET DEFAULT nextval('public."сountry_id_country_seq"'::regclass);
--
-- Name: Customer id_customer; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Customer" ALTER COLUMN id_customer SET DEFAULT nextval('public."Сustomer_id_customer_seq"'::regclass);
--
-- Name: Item id_item; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Item" ALTER COLUMN id_item SET DEFAULT nextval('public."Item_id_item_seq"'::regclass);
--
-- Name: Provider id_provider; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Provider" ALTER COLUMN id_provider SET DEFAULT nextval('public.provider_id_provider_seq'::regclass);
--
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);
--
-- Data for Name: Attribute; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: Attribute_value; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: Category; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: Company; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: Contract; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: Country; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: Customer; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: Item; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: Provider; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--
--
-- Name: Attribute_id_attribute_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public."Attribute_id_attribute_seq"', 26, true);
--
-- Name: Attribute_value_id_attribute_value_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public."Attribute_value_id_attribute_value_seq"', 86, true);
--
-- Name: Category_id_category_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public."Category_id_category_seq"', 212, true);
--
-- Name: Contract_id_contract_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public."Contract_id_contract_seq"', 13, true);
--
-- Name: Item_id_item_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public."Item_id_item_seq"', 100009, true);
--
-- Name: company_id_company_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public.company_id_company_seq', 159, true);
--
-- Name: provider_id_provider_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public.provider_id_provider_seq', 22, true);
--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public.users_id_seq', 26, true);
--
-- Name: Сustomer_id_customer_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public."Сustomer_id_customer_seq"', 25, true);
--
-- Name: сountry_id_country_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--
SELECT pg_catalog.setval('public."сountry_id_country_seq"', 478, true);
--
-- Name: Attribute Attribute_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Attribute"
    ADD CONSTRAINT "Attribute_pk" PRIMARY KEY (id_attribute);
--
-- Name: Attribute_value Attribute_value_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Attribute_value"
    ADD CONSTRAINT "Attribute_value_pkey" PRIMARY KEY (id_attribute_value);
--
-- Name: Category Category_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Category"
    ADD CONSTRAINT "Category_pk" PRIMARY KEY (id_category);
--
-- Name: Contract Contract_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Contract"
    ADD CONSTRAINT "Contract_pk" PRIMARY KEY (id_contract);
--
-- Name: Customer Customer_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Customer"
    ADD CONSTRAINT "Customer_pk" PRIMARY KEY (id_customer);
--
-- Name: Customer check.phone.number; Type: CHECK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE public."Customer"
    ADD CONSTRAINT "check.phone.number" CHECK (((phone_number)::text ~ '^\+\d+$'::text)) NOT VALID;
--
-- Name: Company company_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Company"
    ADD CONSTRAINT company_pkey PRIMARY KEY (id_company);
--
-- Name: Item item_pk; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Item"
    ADD CONSTRAINT item_pk PRIMARY KEY (id_item);
--
-- Name: Provider provider_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Provider"
    ADD CONSTRAINT provider_pkey PRIMARY KEY (id_provider);
--
-- Name: Category uniq_category_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Category"
    ADD CONSTRAINT uniq_category_name UNIQUE (category_name);
--
-- Name: Company uniq_company_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Company"
    ADD CONSTRAINT uniq_company_name UNIQUE (company_name);
--
-- Name: Country uniq_country_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Country"
    ADD CONSTRAINT uniq_country_name UNIQUE (country_name);
--
-- Name: Attribute unique_category_attribute; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Attribute"
    ADD CONSTRAINT unique_category_attribute UNIQUE (id_category, attribute_name);
--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
--
-- Name: Country сountry_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Country"
    ADD CONSTRAINT "сountry_pkey" PRIMARY KEY (id_country);
--
-- Name: Attribute Attribute_id_category_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Attribute"
    ADD CONSTRAINT "Attribute_id_category_fk" FOREIGN KEY (id_category) REFERENCES public."Category"(id_category) ON UPDATE CASCADE ON DELETE CASCADE;
--
-- Name: Attribute_value Attribute_value_id_item_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Attribute_value"
    ADD CONSTRAINT "Attribute_value_id_item_fk" FOREIGN KEY (id_item) REFERENCES public."Item"(id_item) NOT VALID;
--
-- Name: Contract Contract_id_customer_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Contract"
    ADD CONSTRAINT "Contract_id_customer_fk" FOREIGN KEY (id_customer) REFERENCES public."Customer"(id_customer) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
--
-- Name: Contract Contract_id_item_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Contract"
    ADD CONSTRAINT "Contract_id_item_fk" FOREIGN KEY (id_item) REFERENCES public."Item"(id_item) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
--
-- Name: Item Item_id_category_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Item"
    ADD CONSTRAINT "Item_id_category_fk" FOREIGN KEY (id_category) REFERENCES public."Category"(id_category) ON UPDATE CASCADE ON DELETE CASCADE;
--
-- Name: Item Item_id_provider_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Item"
    ADD CONSTRAINT "Item_id_provider_fk" FOREIGN KEY (id_provider) REFERENCES public."Provider"(id_provider) ON UPDATE CASCADE ON DELETE CASCADE;
--
-- Name: Provider Provider_id_company_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Provider"
    ADD CONSTRAINT "Provider_id_company_fk" FOREIGN KEY (id_company) REFERENCES public."Company"(id_company) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
--
-- Name: Provider Provider_id_country_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--
ALTER TABLE ONLY public."Provider"
    ADD CONSTRAINT "Provider_id_country_fk" FOREIGN KEY (id_country) REFERENCES public."Country"(id_country) ON UPDATE CASCADE ON DELETE CASCADE NOT VALID;
--
-- Name: users; Type: ROW SECURITY; Schema: public; Owner: postgres
--
ALTER TABLE public.users ENABLE ROW LEVEL SECURITY;
--
-- Name: users users_insert_pol; Type: POLICY; Schema: public; Owner: postgres
--
--
-- Name: users users_view_pol; Type: POLICY; Schema: public; Owner: postgres
--
CREATE POLICY users_view_pol ON public.users FOR SELECT TO "Admin", "Employee" USING (true);
--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: pg_database_owner
--
GRANT USAGE ON SCHEMA public TO "Admin" WITH GRANT OPTION;
GRANT USAGE ON SCHEMA public TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE "Attribute"; Type: ACL; Schema: public; Owner: postgres
--
GRANT SELECT,UPDATE ON TABLE public."Attribute" TO "Employee" WITH GRANT OPTION;
--
-- Name: SEQUENCE "Attribute_id_attribute_seq"; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public."Attribute_id_attribute_seq" TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public."Attribute_id_attribute_seq" TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE "Attribute_value"; Type: ACL; Schema: public; Owner: postgres
--
--
-- Name: SEQUENCE "Attribute_value_id_attribute_value_seq"; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public."Attribute_value_id_attribute_value_seq" TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public."Attribute_value_id_attribute_value_seq" TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE "Category"; Type: ACL; Schema: public; Owner: postgres
--
GRANT SELECT ON TABLE public."Category" TO "Employee" WITH GRANT OPTION;
--
-- Name: SEQUENCE "Category_id_category_seq"; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public."Category_id_category_seq" TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public."Category_id_category_seq" TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE "Company"; Type: ACL; Schema: public; Owner: postgres
--
GRANT SELECT ON TABLE public."Company" TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE "Contract"; Type: ACL; Schema: public; Owner: postgres
--
--
-- Name: SEQUENCE "Contract_id_contract_seq"; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public."Contract_id_contract_seq" TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public."Contract_id_contract_seq" TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE "Country"; Type: ACL; Schema: public; Owner: postgres
--
GRANT SELECT ON TABLE public."Country" TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE "Customer"; Type: ACL; Schema: public; Owner: postgres
--
--
-- Name: TABLE "Item"; Type: ACL; Schema: public; Owner: postgres
--
--
-- Name: SEQUENCE "Item_id_item_seq"; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public."Item_id_item_seq" TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public."Item_id_item_seq" TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE "Provider"; Type: ACL; Schema: public; Owner: postgres
--
GRANT SELECT ON TABLE public."Provider" TO "Employee" WITH GRANT OPTION;
--
-- Name: SEQUENCE company_id_company_seq; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public.company_id_company_seq TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public.company_id_company_seq TO "Employee" WITH GRANT OPTION;
--
-- Name: SEQUENCE provider_id_provider_seq; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public.provider_id_provider_seq TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public.provider_id_provider_seq TO "Employee" WITH GRANT OPTION;
--
-- Name: TABLE users; Type: ACL; Schema: public; Owner: postgres
--
GRANT SELECT ON TABLE public.users TO "Employee" WITH GRANT OPTION;
GRANT ALL ON TABLE public.users TO "Admin";
--
-- Name: SEQUENCE users_id_seq; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public.users_id_seq TO "Admin";
GRANT ALL ON SEQUENCE public.users_id_seq TO "Employee";
--
-- Name: SEQUENCE "Сustomer_id_customer_seq"; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public."Сustomer_id_customer_seq" TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public."Сustomer_id_customer_seq" TO "Employee" WITH GRANT OPTION;
--
-- Name: SEQUENCE "сountry_id_country_seq"; Type: ACL; Schema: public; Owner: postgres
--
GRANT ALL ON SEQUENCE public."сountry_id_country_seq" TO "Admin" WITH GRANT OPTION;
GRANT ALL ON SEQUENCE public."сountry_id_country_seq" TO "Employee" WITH GRANT OPTION;

