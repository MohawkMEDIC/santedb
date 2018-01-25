﻿/** 
 * <update id="20170721-01" applyRange="0.2.0.0-0.9.0.0" invariantName="npgsql">
 *	<summary>Installs additional entity relationships to database</summary>
 *	<remarks>This update will install two new entity relationship types: INSTANCE and LOCATED ENTITY. INSTANCE replaces the old MANUFACTURED_PRODUCT relationship
 *	type, and is used to indicate a manufactured material is an instance of a material kind. LOCATED ENTITY is used to indicate that a particular entity is physically
 *	located in another entity</remarks>
 *  <isInstalled>select string_to_array(get_sch_vrsn(), '.')::int[] >= string_to_array('0.9.0.1', '.')::int[]</isInstalled>
 * </update>
 */
BEGIN TRANSACTION;


CREATE TABLE PATCH_DB_SYSTBL (
	PATCH_ID	VARCHAR(24) NOT NULL, 
	APPLY_DATE	TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT PK_PTCH_DB_SYSTBL PRIMARY KEY (PATCH_ID)
);

CREATE OR REPLACE FUNCTION REG_PATCH(PATCH_ID_IN IN VARCHAR(24)) RETURNS BOOLEAN AS $$
BEGIN
	INSERT INTO PATCH_DB_SYSTBL (PATCH_ID) VALUES (PATCH_ID_IN);
	RETURN TRUE;
END
$$ LANGUAGE PLPGSQL;

CREATE OR REPLACE FUNCTION CK_PATCH(PATCH_ID_IN IN VARCHAR(24)) RETURNS BOOLEAN AS $$
BEGIN
	RETURN (SELECT COUNT(*) > 0 FROM PATCH_DB_SYSTBL WHERE PATCH_ID = PATCH_ID_IN);
END
$$ LANGUAGE PLPGSQL;

DROP INDEX CD_VRSN_MNEMONIC_IDX;
CREATE UNIQUE INDEX CD_VRSN_MNEMONIC_IDX ON CD_VRSN_TBL(MNEMONIC) WHERE OBSLT_UTC IS NULL;

-- FIX FOR INSTANCE CODE TYPE
INSERT INTO CD_TBL VALUES ('AC45A740-B0C7-4425-84D8-B3F8A41FEF9F', TRUE);
INSERT INTO CD_SET_MEM_ASSOC_TBL (CD_ID, SET_ID) VALUES ('AC45A740-B0C7-4425-84D8-B3F8A41FEF9F', '5285af4b-e060-41b7-99d9-fd8237e5aecf');
 INSERT INTO CD_SET_MEM_ASSOC_TBL (CD_ID, SET_ID) VALUES ('AC45A740-B0C7-4425-84D8-B3F8A41FEF9F', 'ee16a667-2085-440a-b1e7-4032d10b9f40');
INSERT INTO CD_VRSN_TBL (CD_ID, STS_CD_ID, CRT_USR_ID, MNEMONIC, CLS_ID) VALUES ('AC45A740-B0C7-4425-84D8-B3F8A41FEF9F', 'c8064cbd-fa06-4530-b430-1a52f1530c27', 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8', 'Instance', '17fd5254-8c25-4abb-b246-083fbe9afa15');
INSERT INTO REF_TERM_TBL (REF_TERM_ID, CS_ID, MNEMONIC, CRT_USR_ID)  VALUES('9830f62b-732f-434f-be24-dd5ff96c4ed3','ff12d77b-625d-497a-8794-c3726942b113','INST', 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8');
INSERT INTO CD_REF_TERM_ASSOC_TBL (REF_TERM_ID, CD_ID, EFFT_VRSN_SEQ_ID, REL_TYP_ID) SELECT '9830f62b-732f-434f-be24-dd5ff96c4ed3', 'AC45A740-B0C7-4425-84D8-B3F8A41FEF9F', VRSN_SEQ_ID, '2c4dafc2-566a-41ae-9ebc-3097d7d22f4a' FROM CD_VRSN_TBL WHERE CD_ID = 'AC45A740-B0C7-4425-84D8-B3F8A41FEF9F' AND OBSLT_UTC IS NULL;
INSERT INTO REF_TERM_NAME_TBL (REF_TERM_ID, LANG_CS, TERM_NAME, CRT_USR_ID, PHON_ALG_ID) VALUES ('9830f62b-732f-434f-be24-dd5ff96c4ed3', 'en', 'instance', 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8', '402CD339-D0E4-46CE-8FC2-12A4B0E17226');
INSERT INTO CD_NAME_TBL (CD_ID, EFFT_VRSN_SEQ_ID, LANG_CS, VAL, PHON_ALG_ID) SELECT 'AC45A740-B0C7-4425-84D8-B3F8A41FEF9F', VRSN_SEQ_ID, 'en', 'instance', '402CD339-D0E4-46CE-8FC2-12A4B0E17226' FROM CD_VRSN_TBL WHERE CD_ID = 'AC45A740-B0C7-4425-84D8-B3F8A41FEF9F' AND OBSLT_UTC IS NULL;

-- FIX FOR INSTANCE CODE TYPE
INSERT INTO CD_TBL VALUES ('4F6273D3-8223-4E18-8596-C718AD029DEB', TRUE);
INSERT INTO CD_SET_MEM_ASSOC_TBL (CD_ID, SET_ID) VALUES ('4F6273D3-8223-4E18-8596-C718AD029DEB', '5285af4b-e060-41b7-99d9-fd8237e5aecf');
INSERT INTO CD_SET_MEM_ASSOC_TBL (CD_ID, SET_ID) VALUES ('4F6273D3-8223-4E18-8596-C718AD029DEB', 'ee16a667-2085-440a-b1e7-4032d10b9f40');
INSERT INTO CD_VRSN_TBL (CD_ID, STS_CD_ID, CRT_USR_ID, MNEMONIC, CLS_ID) VALUES ('4F6273D3-8223-4E18-8596-C718AD029DEB', 'c8064cbd-fa06-4530-b430-1a52f1530c27', 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8', 'LocatedEntity', '17fd5254-8c25-4abb-b246-083fbe9afa15');
INSERT INTO REF_TERM_TBL (REF_TERM_ID, CS_ID, MNEMONIC, CRT_USR_ID)  VALUES('7ad0f62b-732f-434f-be24-dd5ff96c4ed3','ff12d77b-625d-497a-8794-c3726942b113','LOCE', 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8');
INSERT INTO CD_REF_TERM_ASSOC_TBL (REF_TERM_ID, CD_ID, EFFT_VRSN_SEQ_ID, REL_TYP_ID) SELECT '7ad0f62b-732f-434f-be24-dd5ff96c4ed3', '4F6273D3-8223-4E18-8596-C718AD029DEB', VRSN_SEQ_ID, '2c4dafc2-566a-41ae-9ebc-3097d7d22f4a' FROM CD_VRSN_TBL WHERE CD_ID = '4F6273D3-8223-4E18-8596-C718AD029DEB' AND OBSLT_UTC IS NULL;
INSERT INTO REF_TERM_NAME_TBL (REF_TERM_ID, LANG_CS, TERM_NAME, CRT_USR_ID, PHON_ALG_ID) VALUES ('7ad0f62b-732f-434f-be24-dd5ff96c4ed3', 'en', 'located entity', 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8', '402CD339-D0E4-46CE-8FC2-12A4B0E17226');
INSERT INTO CD_NAME_TBL (CD_ID, EFFT_VRSN_SEQ_ID, LANG_CS, VAL, PHON_ALG_ID) SELECT '4F6273D3-8223-4E18-8596-C718AD029DEB', VRSN_SEQ_ID, 'en', 'located entity', '402CD339-D0E4-46CE-8FC2-12A4B0E17226' FROM CD_VRSN_TBL WHERE CD_ID = '4F6273D3-8223-4E18-8596-C718AD029DEB' AND OBSLT_UTC IS NULL;

-------------------------------------------------
-- FIRST MANUFACTURED PRODUCTS BECOME INSTANCE --
-------------------------------------------------

-- OBSOLETE ALL MANUFACTURED PRODUCTS
UPDATE ENT_REL_TBL SET OBSLT_VRSN_SEQ_ID = (SELECT VRSN_SEQ_ID FROM ENT_VRSN_TBL WHERE ENT_VRSN_TBL.ENT_ID = SRC_ENT_ID AND OBSLT_UTC IS NULL) WHERE REL_TYP_CD_ID = '6780DF3B-AFBD-44A3-8627-CBB3DC2F02F6';

-- INSERT NEW VERSIONS
INSERT INTO ENT_VRSN_TBL 
	(ent_id, rplc_vrsn_id, sts_cd_id, typ_cd_id, crt_usr_id)
	SELECT ent_id, ent_vrsn_id, 'c34fcbf1-e0fe-4989-90fd-0dc49e1b9685', typ_cd_id, 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8'
	FROM ENT_VRSN_TBL 
	WHERE ENT_ID IN (SELECT SRC_ENT_ID FROM ENT_REL_TBL WHERE REL_TYP_CD_ID = '6780DF3B-AFBD-44A3-8627-CBB3DC2F02F6') AND OBSLT_UTC IS NULL AND sts_cd_id = 'c8064cbd-fa06-4530-b430-1a52f1530c27';

-- FIND THE NEW VERSIONS THAT WERE INSERTED AND CREATE SUPPORTING TABLE
INSERT INTO MAT_TBL
	(ENT_VRSN_ID, EXP_UTC, FRM_CD_ID, QTY, QTY_CD_ID, IS_ADM)
	SELECT (SELECT ent_vrsn_id FROM ENT_VRSN_TBL B WHERE B.ENT_ID = ENT_VRSN_TBL.ENT_ID AND sts_cd_id = 'c34fcbf1-e0fe-4989-90fd-0dc49e1b9685'), EXP_UTC, FRM_CD_ID, QTY, QTY_CD_ID, IS_ADM FROM MAT_TBL NATURAL JOIN ENT_VRSN_TBL 
	WHERE ENT_ID IN (SELECT SRC_ENT_ID FROM ENT_REL_TBL WHERE REL_TYP_CD_ID = '6780DF3B-AFBD-44A3-8627-CBB3DC2F02F6') AND OBSLT_UTC IS NULL AND sts_cd_id = 'c8064cbd-fa06-4530-b430-1a52f1530c27';

-- OBSOLETE OLD VERSIONS
UPDATE ENT_VRSN_TBL SET OBSLT_UTC = CURRENT_TIMESTAMP, OBSLT_USR_ID = 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8' WHERE ENT_ID IN (SELECT SRC_ENT_ID FROM ENT_REL_TBL WHERE REL_TYP_CD_ID = '6780DF3B-AFBD-44A3-8627-CBB3DC2F02F6') AND OBSLT_UTC IS NULL AND sts_cd_id = 'c8064cbd-fa06-4530-b430-1a52f1530c27';

-- ACTIVATE NEW VERSIONS
UPDATE ENT_VRSN_TBL SET STS_CD_ID = 'c8064cbd-fa06-4530-b430-1a52f1530c27' WHERE ENT_ID IN (SELECT SRC_ENT_ID FROM ENT_REL_TBL WHERE REL_TYP_CD_ID = '6780DF3B-AFBD-44A3-8627-CBB3DC2F02F6') AND OBSLT_UTC IS NULL AND sts_cd_id = 'c34fcbf1-e0fe-4989-90fd-0dc49e1b9685';

-- INSERT NEW RELATIONSHIPS WITH TYPE INSTANCE
INSERT INTO ENT_REL_TBL 
	(src_ent_id, trg_ent_id, efft_vrsn_seq_id, rel_typ_cd_id, qty) 
	SELECT src_ent_id, trg_ent_id, vrsn_seq_id, 'AC45A740-B0C7-4425-84D8-B3F8A41FEF9F' , qty
	FROM ENT_REL_TBL INNER JOIN ENT_VRSN_TBL ON (ENT_VRSN_TBL.ENT_ID = ENT_REL_TBL.SRC_ENT_ID) 
	WHERE OBSLT_VRSN_SEQ_ID IS NOT NULL AND REL_TYP_CD_ID = '6780DF3B-AFBD-44A3-8627-CBB3DC2F02F6' AND OBSLT_UTC IS NULL;

	
----------------------------------------------------------
-- NEXT WARRANTED PRODUCTS BECOME MANUFACTURED PRODUCTS --
----------------------------------------------------------

-- OBSOLETE ALL WARRANTED PRODUCTS
UPDATE ENT_REL_TBL SET OBSLT_VRSN_SEQ_ID = (SELECT VRSN_SEQ_ID FROM ENT_VRSN_TBL WHERE ENT_VRSN_TBL.ENT_ID = SRC_ENT_ID AND OBSLT_UTC IS NULL) WHERE REL_TYP_CD_ID = '639B4B8F-AFD3-4963-9E79-EF0D3928796A';

-- INSERT NEW VERSIONS
INSERT INTO ENT_VRSN_TBL 
	(ent_id, rplc_vrsn_id, sts_cd_id, typ_cd_id, crt_usr_id)
	SELECT ent_id, ent_vrsn_id, 'c34fcbf1-e0fe-4989-90fd-0dc49e1b9685', typ_cd_id, 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8'
	FROM ENT_VRSN_TBL 
	WHERE ENT_ID IN (SELECT SRC_ENT_ID FROM ENT_REL_TBL WHERE REL_TYP_CD_ID = '639B4B8F-AFD3-4963-9E79-EF0D3928796A') AND OBSLT_UTC IS NULL AND sts_cd_id = 'c8064cbd-fa06-4530-b430-1a52f1530c27';

	
-- FIND THE NEW VERSIONS THAT WERE INSERTED AND CREATE SUPPORTING TABLE
INSERT INTO ORG_TBL
	(ENT_VRSN_ID, IND_CD_ID)
	SELECT (SELECT ent_vrsn_id FROM ENT_VRSN_TBL B WHERE B.ENT_ID = ENT_VRSN_TBL.ENT_ID AND sts_cd_id = 'c34fcbf1-e0fe-4989-90fd-0dc49e1b9685'), IND_CD_ID FROM ORG_TBL NATURAL JOIN ENT_VRSN_TBL 
	WHERE ENT_ID IN (SELECT SRC_ENT_ID FROM ENT_REL_TBL WHERE REL_TYP_CD_ID = '639B4B8F-AFD3-4963-9E79-EF0D3928796A') AND OBSLT_UTC IS NULL AND sts_cd_id = 'c8064cbd-fa06-4530-b430-1a52f1530c27';

-- OBSOLETE OLD VERSIONS
UPDATE ENT_VRSN_TBL SET OBSLT_UTC = CURRENT_TIMESTAMP, OBSLT_USR_ID = 'fadca076-3690-4a6e-af9e-f1cd68e8c7e8' WHERE ENT_ID IN (SELECT SRC_ENT_ID FROM ENT_REL_TBL WHERE REL_TYP_CD_ID = '639B4B8F-AFD3-4963-9E79-EF0D3928796A') AND OBSLT_UTC IS NULL AND sts_cd_id = 'c8064cbd-fa06-4530-b430-1a52f1530c27';

-- ACTIVATE NEW VERSIONS
UPDATE ENT_VRSN_TBL SET STS_CD_ID = 'c8064cbd-fa06-4530-b430-1a52f1530c27' WHERE ENT_ID IN (SELECT SRC_ENT_ID FROM ENT_REL_TBL WHERE REL_TYP_CD_ID = '639B4B8F-AFD3-4963-9E79-EF0D3928796A') AND OBSLT_UTC IS NULL AND sts_cd_id = 'c34fcbf1-e0fe-4989-90fd-0dc49e1b9685';

-- INSERT NEW RELATIONSHIPS WITH TYPE INSTANCE
INSERT INTO ENT_REL_TBL 
	(src_ent_id, trg_ent_id, efft_vrsn_seq_id, rel_typ_cd_id) 
	SELECT src_ent_id, trg_ent_id, vrsn_seq_id, '6780DF3B-AFBD-44A3-8627-CBB3DC2F02F6' 
	FROM ENT_REL_TBL INNER JOIN ENT_VRSN_TBL ON (ENT_VRSN_TBL.ENT_ID = ENT_REL_TBL.SRC_ENT_ID) 
	WHERE OBSLT_VRSN_SEQ_ID IS NOT NULL AND REL_TYP_CD_ID = '639B4B8F-AFD3-4963-9E79-EF0D3928796A' AND OBSLT_UTC IS NULL;

-- GET THE SCHEMA VERSION
CREATE OR REPLACE FUNCTION GET_SCH_VRSN() RETURNS VARCHAR(10) AS
$$
BEGIN
	RETURN '0.9.0.1';
END;
$$ LANGUAGE plpgsql;

SELECT REG_PATCH('20170721-01');

COMMIT;
