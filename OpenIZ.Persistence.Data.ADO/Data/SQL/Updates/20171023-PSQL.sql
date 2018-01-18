﻿/** 
 * <update id="20171023-01" applyRange="0.2.0.4-0.9.0.6"  invariantName="npgsql">
 *	<summary>Adds the Procedure table to the OpenIZ schema</summary>
 *	<remarks></remarks>
 *	<guard>select not(ck_patch('20171023-01'))</guard>
 * </update>
 */

BEGIN TRANSACTION ;

CREATE TABLE PROC_TBL (
	ACT_VRSN_ID	UUID NOT NULL, -- THE UUID FOR THE ACT VERSION INFORMATION
	MTH_CD_ID UUID, -- THE UUID FOR THE METHOD / TECHNIQUE CODE
	APR_STE_CD_ID UUID, -- THE UUID FOR THE APPOACH SITE CODE
	TRG_STE_CD_ID UUID, -- THE UUID FOR THE TARGET SITE CODE
	CONSTRAINT PK_PROC_TBL PRIMARY KEY (ACT_VRSN_ID),
	CONSTRAINT FK_PROC_VRSN_ID FOREIGN KEY (ACT_VRSN_ID) REFERENCES ACT_VRSN_TBL(ACT_VRSN_ID),
	CONSTRAINT FK_PROC_MTH_CD_ID FOREIGN KEY (MTH_CD_ID) REFERENCES CD_TBL(CD_ID),
	CONSTRAINT FK_PROC_APR_STE_CD_ID FOREIGN KEY (APR_STE_CD_ID) REFERENCES CD_TBL(CD_ID),
	CONSTRAINT FK_PROC_TRG_STE_CD_ID FOREIGN KEY (TRG_STE_CD_ID) REFERENCES CD_TBL(CD_ID),
	CONSTRAINT CK_PROC_MTH_CD_ID CHECK (MTH_CD_ID IS NULL OR CK_IS_CD_SET_MEM(MTH_CD_ID, 'ProcedureTechniqueCode', TRUE)),
	CONSTRAINT CK_PROC_APR_STE_CD_ID CHECK(APR_STE_CD_ID IS NULL OR CK_IS_CD_SET_MEM(APR_STE_CD_ID, 'BodySiteOrSystemCode', TRUE)),
	CONSTRAINT CK_PROC_TRG_STE_CD_ID CHECK (TRG_STE_CD_ID IS NULL OR CK_IS_CD_SET_MEM(TRG_STE_CD_ID, 'BodySiteOrSystemCode', TRUE))
);

 -- GET THE SCHEMA VERSION
CREATE OR REPLACE FUNCTION GET_SCH_VRSN() RETURNS VARCHAR(10) AS
$$
BEGIN
	RETURN '0.9.0.8';
END;
$$ LANGUAGE plpgsql;

SELECT REG_PATCH('20171023-01');

COMMIT;