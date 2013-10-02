/*
   Copyright 2013 Aileron Consulting LLC

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Category_I;

namespace Grau.Core
{
    public class QrdaCategoryICreator
    {
        public const string MeasureSectionTemplateIdRootQdm = "2.16.840.1.113883.10.20.24.2.3";
        public const string ReportingParametersSectionTemplateIdRoot = "2.16.840.1.113883.10.20.17.2.1";
        public const string PatientDataSectionTemplateIdRootQdm = "2.16.840.1.113883.10.20.24.2.1";
        public const string MeasureSectionTemplateIdRoot = "2.16.840.1.113883.10.20.24.2.2";
        public const string PatientDataSectionTemplateIdRoot = "2.16.840.1.113883.10.20.17.2.4";
        public const string CdaQrdaTemplateId = "2.16.840.1.113883.10.20.22.1.1";
        //US Realm Header Template Id
        public const string CdaUsRealmHeaderTemplateId = "2.16.840.1.113883.10.20.22.1.1";
        //QDM-based QRDA templateId
        public const string CdaQdmBasedQrdaTemplateId = "2.16.840.1.113883.10.20.24.1.2";

        public const string CdaIdRoot = "2.16.840.1.113883.4.572";
        public const string OrganizationIdRootTaxIdNumber = "2.16.840.1.113883.4.2";
        public const string OrganizationIdRootFacilityCmsCertificationNumber = "2.16.840.1.113883.4.336";
        private const string NationalProviderIdentifierIdRoot = "2.16.840.1.113883.4.336";
        public const string PatientRoleDefaultIdRoot = "2.16.840.1.113883.4.572";
        private const string DateTimeFormatToDay = "yyyyMMdd";
        private const string DateTimeFormatToMinute = "yyyyMMddmm";
        private const string DateTimeFormatToSecond = "yyyyMMddmmss";
        private bool _copyCodeTitle;
        public QrdaCategoryIDto QrdaCdaDto { get; set; }

        public POCD_MT000040ClinicalDocument GetQrdacdaFromDto()
        {
            var cda = new POCD_MT000040ClinicalDocument();
            AddGeneralHeaderInfo(cda);
            AddDocumentLevelInfo(cda);
            return cda;
        }

        private void AddDocumentLevelInfo(POCD_MT000040ClinicalDocument cda)
        {
            cda.component = new POCD_MT000040Component2();
            object item = null;
            if (QrdaCdaDto.Body == null)
            {
                item = new POCD_MT000040NonXMLBody();
            }
            else
            {
                var body = CreateStructureBody(QrdaCdaDto.Body);
            }

            cda.component.Item = item;
        }

        private POCD_MT000040StructuredBody CreateStructureBody(ComponentBody componentBody)
        {
            var body = new POCD_MT000040StructuredBody();
            if (componentBody.MeasureSection == null)
            {
                throw new Exception(
                    "This structuredBody SHALL contain exactly one [1..1] Measure Section (templateId:2.16.840.1.113883.10.20.24.2.2)(CONF:12920).");
            }
            if (componentBody.PatientDataSection == null)
            {
                throw new Exception(
                    "This structuredBody SHALL contain exactly one [1..1] Reporting Parameters Section (templateId:2.16.840.1.113883.10.20.17.2.1) (CONF:12923)");
            }
            if (componentBody.ReportingParametersSection == null)
            {
                throw new Exception(
                    "This structuredBody SHALL contain exactly one [1..1] Reporting Parameters Section (templateId:2.16.840.1.113883.10.20.17.2.1) (CONF:12923)");
            }
            body.component = new POCD_MT000040Component3[3];
            body.component[0] = CreateMeasureSection(componentBody.MeasureSection);
            body.component[1] = CreateReportingParameterSection(componentBody.ReportingParametersSection);
            body.component[2] = CreatePatientDataSection(componentBody.PatientDataSection);

            return body;
        }

        private POCD_MT000040Component3 CreatePatientDataSection(PatientDataSection patientDataSection)
        {
            var patient = new POCD_MT000040Component3
                {
                    templateId = new II[2]
                        {
                            new II {root = PatientDataSectionTemplateIdRoot},
                            new II {root = PatientDataSectionTemplateIdRootQdm}
                        }
                };

            return patient;
        }

        private POCD_MT000040Component3 CreateReportingParameterSection(
            ReportingParametersSection reportingParametersSection)
        {
            var rps = new POCD_MT000040Component3
                {
                    templateId = new II[1] {new II {root = ReportingParametersSectionTemplateIdRoot}}
                };
            return rps;
        }

        private POCD_MT000040Component3 CreateMeasureSection(MeasureSection measureSection)
        {
            var measure = new POCD_MT000040Component3();
            var section = new POCD_MT000040Section();
            section.templateId = new II[2]
                {new II {root = MeasureSectionTemplateIdRoot}, new II {root = MeasureSectionTemplateIdRootQdm}};
            section.code = new CE {codeSystem = "2.16.840.1.113883.6.1", code = "55186-1", codeSystemName = "LOINC"};
            section.title = new ST {Text = new[] {"Measure Section"}};


            measure.section = section;
            return measure;
        }

        //QRDA templateId 

        private void AddGeneralHeaderInfo(POCD_MT000040ClinicalDocument cda)
        {
            //CONF:16791
            cda.realmCode = new CS[1];
            cda.realmCode[0] = new CS {code = "US"};
            //CONF:5361
            //CONF:5250
            //CONF:5251
            //CONF:5252
            cda.typeId = new POCD_MT000040InfrastructureRoottypeId
                {
                    root = "2.16.840.1.113883.1.3",
                    extension = "POCD_HD000040"
                };
            cda.templateId = new II[3];
            //CONF:10036
            cda.templateId[0] = new II {root = CdaUsRealmHeaderTemplateId};
            cda.templateId[1] = new II {root = CdaQrdaTemplateId};
            cda.templateId[2] = new II {root = CdaQdmBasedQrdaTemplateId};
            //CONF:5363
            cda.id = new II();
            //CONF:9991
            cda.id.root = CdaIdRoot;
            //CONF:5253
            cda.code = new CE
                {
                    code = "55182-0",
                    codeSystem = "2.16.840.1.113883.6.1",
                    codeSystemName = "LOINC"
                };
            //CONF:9992
            //TODO CONF:5254
            //CONF:5254
            if (!string.IsNullOrEmpty(QrdaCdaDto.CdaTitle))
            {
                cda.title = new ST {Text = new string[1]};
                cda.title.Text[0] = QrdaCdaDto.CdaTitle;
            }
            else
            {
                //CONF:5255
                //TODO Copy Code Title
                _copyCodeTitle = true;
                cda.title = new ST();
                cda.title.Text[0] = string.Empty;
            }
            //CONF:16865
            UsRealDateAndTime effectiveTimeDate = QrdaCdaDto.EffectiveTimeDate;
            //CONF:5256
            cda.effectiveTime = CreateUsRealDateTime(effectiveTimeDate);
            //CONF:5259
            cda.confidentialityCode = new CE
                {
                    code = QrdaCdaDto.ConfidentialityCode.Code,
                    codeSystem = QrdaCdaDto.ConfidentialityCode.CodeSystem
                };
            //CONF:5372
            cda.languageCode = new CS
                {
                    code = QrdaCdaDto.LanguageCode.CodeSystem,
                };
            //CONF:5261
            if (!string.IsNullOrEmpty(QrdaCdaDto.CdaSetId))
            {
                cda.setId = new II {root = QrdaCdaDto.CdaSetId};
                //CONF:6380
                if (string.IsNullOrEmpty(QrdaCdaDto.VersionNumber))
                {
                    throw new Exception("If setId is present versionNumber SHALL be present (CONF:6380).");
                }
                cda.versionNumber = new INT {value = QrdaCdaDto.VersionNumber};
            }

            if (!string.IsNullOrEmpty(QrdaCdaDto.VersionNumber))
            {
                cda.versionNumber = new INT {value = QrdaCdaDto.VersionNumber};
                if (string.IsNullOrEmpty(QrdaCdaDto.CdaSetId))
                {
                    throw new Exception("If versionNumber is present setId SHALL be present (CONF:6387).");
                }
                cda.setId = new II {root = QrdaCdaDto.CdaSetId};
            }
            //CONF:5266
            if (QrdaCdaDto.RecordTarget == null)
                throw new Exception(
                    "There should be at least one record target. contain at least one [1..1] recordTarget (CONF:5266)");

            cda.recordTarget = new POCD_MT000040RecordTarget[1];
            for (int i = 0; i < cda.recordTarget.Length; ++i)
            {
                cda.recordTarget[i] = new POCD_MT000040RecordTarget();
                AddPatientRole(cda.recordTarget[i], QrdaCdaDto.RecordTarget);
            }

            //(CONF:5444)
            if (QrdaCdaDto.Authors == null || QrdaCdaDto.Authors.Count == 0)
            {
                throw new Exception("SHALL contain at least one [1..*] author (CONF:5444)");
            }
            List<Author> authors = QrdaCdaDto.Authors;
            int index = 0;
            cda.author = new POCD_MT000040Author[authors.Count];
            foreach (Author author in authors)
            {
                cda.author[index] = CreateAuthor(author);
                index++;
            }
            //CONF:5441
            if (QrdaCdaDto.DataEnterer != null)
            {
                cda.dataEnterer = CreateDataEnterer(QrdaCdaDto.DataEnterer);
            }
            //CONF:8001
            if (QrdaCdaDto.Informants != null && QrdaCdaDto.Informants.Count > 0)
            {
                cda.informant = CreateInformants(QrdaCdaDto.Informants);
            }
            //CONF:5519
            if (QrdaCdaDto.Custodian == null)
            {
                throw new Exception("SHALL contain exactly one [1..1] custodian (CONF:5519).");
            }

            cda.custodian = CreateCustodian(QrdaCdaDto.Custodian);

            //CONF:5565
            if (QrdaCdaDto.InformationRecipients != null && QrdaCdaDto.InformationRecipients.Count > 0)
            {
                cda.informationRecipient = CreateInformationRecipients(QrdaCdaDto.InformationRecipients);
            }

            //CONF:5579
            if (QrdaCdaDto.LegalAuthenticator != null)
            {
                cda.legalAuthenticator = CreateLegalAuthenticator(QrdaCdaDto.LegalAuthenticator);
            }

            //CONF:5607
            if (QrdaCdaDto.Authenticators != null && QrdaCdaDto.Authenticators.Count > 0)
            {
                cda.authenticator = CreateAuthenticators(QrdaCdaDto.Authenticators);
            }

            //CONF:10003
            if (QrdaCdaDto.Participants != null && QrdaCdaDto.Participants.Count > 0)
            {
                cda.participant = CreateParticipants(QrdaCdaDto.Participants);
            }

            //CONF:9952
            if (QrdaCdaDto.InFulfillmentOfs != null && QrdaCdaDto.InFulfillmentOfs.Count > 0)
            {
                cda.inFulfillmentOf = CreateInFulfillmentOfs(QrdaCdaDto.InFulfillmentOfs);
            }
            //CONF:14835
            if (QrdaCdaDto.DocumentationOfs != null && QrdaCdaDto.DocumentationOfs.Count > 0)
            {
                cda.documentationOf = CreateDocumentationOfs(QrdaCdaDto.DocumentationOfs);
            }
            //CONF:16792
            if (QrdaCdaDto.Authorizations != null && QrdaCdaDto.Authorizations.Count > 0)
            {
                cda.authorization = CreateAuthorizations(QrdaCdaDto.Authorizations);
            }

            //CONF:9955
            if (QrdaCdaDto.ComponentOf != null)
            {
                cda.componentOf = CreateComponentOf(QrdaCdaDto.ComponentOf);
            }
        }

        private POCD_MT000040Component1 CreateComponentOf(ComponentOf componentOf)
        {
            var cOf = new POCD_MT000040Component1();
            //CONF:9956
            if (componentOf.EncompassingEncounter == null)
            {
                throw new Exception(
                    "The componentOf, if present, SHALL contain exactly one [1..1] encompassingEncounter (CONF:9956)");
            }
            cOf.encompassingEncounter = CreateEncompassingEncounter(componentOf.EncompassingEncounter);
            return cOf;
        }

        private POCD_MT000040EncompassingEncounter CreateEncompassingEncounter(
            EncompassingEncounter encompassingEncounter)
        {
            var encounter = new POCD_MT000040EncompassingEncounter();
            //CONF:9959
            if (encompassingEncounter.Ids == null || encompassingEncounter.Ids.Count == 0)
            {
                throw new Exception("This encompassingEncounter SHALL contain at least one [1..*] id (CONF:9959).");
            }
            encounter.id = new II[encompassingEncounter.Ids.Count];
            List<string> ids = encompassingEncounter.Ids;
            for (int index = 0; index < ids.Count; index++)
            {
                string id = ids[index];
                encounter.id[index] = new II {root = id};
            }
            //CONF:9958
            if (encompassingEncounter.EffectiveTime == null)
            {
                throw new Exception(
                    "This encompassingEncounter SHALL contain exactly one [1..1] effectiveTime (CONF:9958)");
            }
            encounter.effectiveTime = CreateIntervalTime(encompassingEncounter.EffectiveTime);
            //TODO POCD_MT000040ResponsibleParty
            //TODO POCD_MT000040EncounterParticipant
            //TODO POCD_MT000040Location
            return encounter;
        }

        private POCD_MT000040Authorization[] CreateAuthorizations(List<Authorization> authorizations)
        {
            var auths = new POCD_MT000040Authorization[authorizations.Count];
            for (int index = 0; index < authorizations.Count; index++)
            {
                auths[index] = CreateAuthorization(authorizations[index]);
            }
            return auths;
        }

        private POCD_MT000040Authorization CreateAuthorization(Authorization authorization)
        {
            var auth = new POCD_MT000040Authorization();
            //CONF:16793
            if (authorization.Consent == null)
            {
                throw new Exception("SHALL contain exactly one [1..1] consent (CONF:16793)");
            }
            auth.consent = CreateConsent(authorization.Consent);
            return auth;
        }

        private POCD_MT000040Consent CreateConsent(Consent consent)
        {
            var result = new POCD_MT000040Consent();
            //CONF:16794
            if (consent.Ids != null && consent.Ids.Count > 0)
            {
                result.id = new II[consent.Ids.Count];
                for (int index = 0; index < consent.Ids.Count; index++)
                {
                    string id = consent.Ids[index];
                    result.id[index] = new II {root = id};
                }
            }
            //CONF:16795
            if (consent.Code != null)
            {
                result.code = CreateCodeValueSet(consent.Code);
            }
            //CONF:16797
            //CONF:16798
            result.statusCode = new CS
                {
                    code = consent.StatusCode,
                    codeSystemName = "HL7ActClass",
                    codeSystem = "2.16.840.1.113883.5.6"
                };

            return result;
        }

        private POCD_MT000040DocumentationOf[] CreateDocumentationOfs(List<DocumentationOf> documentationOfs)
        {
            var docsOf = new POCD_MT000040DocumentationOf[documentationOfs.Count];
            for (int index = 0; index < documentationOfs.Count; index++)
            {
                DocumentationOf documentationOf = documentationOfs[index];
                docsOf[index] = CreateDocumentationOf(documentationOf);
            }
            return docsOf;
        }

        private POCD_MT000040DocumentationOf CreateDocumentationOf(DocumentationOf documentationOf)
        {
            var docOf = new POCD_MT000040DocumentationOf();
            //CONF:14836
            if (documentationOf.ServiceEvent == null)
            {
                throw new Exception(
                    "The documentationOf, if present, SHALL contain exactly one [1..1] serviceEvent (CONF:14836)");
            }

            docOf.serviceEvent = CreateServiceEvent(documentationOf.ServiceEvent);

            return docOf;
        }

        private POCD_MT000040ServiceEvent CreateServiceEvent(EventService serviceEvent)
        {
            var sEvent = new POCD_MT000040ServiceEvent();
            sEvent.classCode = "PCPR";
            //CONF:14837
            if (serviceEvent.EffectiveTime == null)
            {
                throw new Exception("This serviceEvent SHALL contain exactly one [1..1] effectiveTime (CONF:14837).");
            }
            //CONF:14838
            if (serviceEvent.EffectiveTime.Low == null)
            {
                throw new Exception("This effectiveTime SHALL contain exactly one [1..1] low(CONF:14838).");
            }
            sEvent.effectiveTime = CreateIntervalTime(serviceEvent.EffectiveTime);

            //CONF:14839
            if (serviceEvent.Performers != null && serviceEvent.Performers.Count > 0)
            {
                sEvent.performer = CreatePerformers(serviceEvent.Performers);
            }


            return sEvent;
        }

        private POCD_MT000040Performer1[] CreatePerformers(List<Performer> performers)
        {
            var pfrmers = new POCD_MT000040Performer1[performers.Count];
            for (int index = 0; index < performers.Count; index++)
            {
                Performer performer = performers[index];
                pfrmers[index] = CreatePerformer(performer);
            }
            return pfrmers;
        }

        private POCD_MT000040Performer1 CreatePerformer(Performer performer)
        {
            var result = new POCD_MT000040Performer1();
            //CONF:14840
            result.typeCode = performer.DefaultTypeCode;
            //CONF:16818
            if (performer.FunctionCode != null)
            {
                result.functionCode = CreateCodeValueSet(performer.FunctionCode);
            }
            //CONF:14841
            if (performer.AssignedEntity == null)
            {
                throw new Exception(
                    "The performer, if present, SHALL contain exactly one [1..1] assignedEntity (CONF:14841).");
            }
            //CONF:14846
            //CONF:14842
            //CONF:14843
            result.assignedEntity = CreateAssignedEntity(performer.AssignedEntity, false, true, true, true);
            result.assignedEntity.representedOrganization =
                CreateRepresentedOrganization(performer.RepresentedOrganization);

            return result;
        }

        private POCD_MT000040Organization CreateRepresentedOrganization(RepresentedOrganization representedOrganization)
        {
            var org = new POCD_MT000040Organization();
            var ids = new List<II>();
            if (representedOrganization.TaxIdNumber != null)
            {
                ids.Add(new II {root = OrganizationIdRootTaxIdNumber, extension = representedOrganization.TaxIdNumber});
            }
            if (representedOrganization.FacilityCmsCertificationNumber != null)
            {
                ids.Add(new II
                    {
                        root = OrganizationIdRootFacilityCmsCertificationNumber,
                        extension = representedOrganization.FacilityCmsCertificationNumber
                    });
            }
            org.id = ids.ToArray();
            return org;
        }

        private POCD_MT000040InFulfillmentOf[] CreateInFulfillmentOfs(List<InFulfillmentOf> inFulfillmentOfs)
        {
            var infs = new POCD_MT000040InFulfillmentOf[inFulfillmentOfs.Count];
            for (int index = 0; index < inFulfillmentOfs.Count; index++)
            {
                InFulfillmentOf inFulfillmentOf = inFulfillmentOfs[index];
                infs[index] = CreateInFulfillmentOf(inFulfillmentOf);
            }

            return infs;
        }

        private POCD_MT000040InFulfillmentOf CreateInFulfillmentOf(InFulfillmentOf inFulfillmentOf)
        {
            var inf = new POCD_MT000040InFulfillmentOf();
            //CONF:9953
            if (inFulfillmentOf.Order == null)
            {
                throw new Exception(
                    "The inFulfillmentOf, if present, SHALL contain exactly one [1..1] order(CONF:9953).");
            }
            inf.order = CreateOrder(inFulfillmentOf.Order);

            return inf;
        }

        private POCD_MT000040Order CreateOrder(Order order)
        {
            //CONF:9954
            if (order.Ids == null || order.Ids.Count == 0)
            {
                throw new Exception("This order SHALL contain at least one [1..*] id (CONF:9954).");
            }
            var ord = new POCD_MT000040Order();
            ord.id = new II[order.Ids.Count];
            for (int index = 0; index < order.Ids.Count; index++)
            {
                string id = order.Ids[index];
                ord.id[index] = new II {root = id};
            }
            return ord;
        }

        private POCD_MT000040Participant1[] CreateParticipants(List<Participant> participants)
        {
            var part = new POCD_MT000040Participant1[participants.Count];
            for (int index = 0; index < participants.Count; ++index)
            {
                part[index] = CreateParticipant(participants[index]);
            }
            return part;
        }

        private POCD_MT000040Participant1 CreateParticipant(Participant participant)
        {
            var part = new POCD_MT000040Participant1();
            //CONF:10004
            if (participant.Time != null)
            {
                part.time = CreateIntervalTime(participant.Time);
            }
            //CONF:10006
            if (participant.AssociatedEntity == null)
            {
                throw new Exception(
                    "Such participants, if present, SHALL have an associatedPerson or scopingOrganization element under participant/associatedEntity (CONF:10006).");
            }
            POCD_MT000040AssociatedEntity assocEntity = CreateAssociatedEntity(participant.AssociatedEntity);
            part.associatedEntity = assocEntity;
            return part;
        }

        private IVL_TS CreateIntervalTime(IntervalTime time)
        {
            var intervalTime = new IVL_TS();
            var items = new List<QTY>();
            if (time.Low != null)
            {
                items.Add(CreateTimeValue(time.Low));
            }
            if (time.High != null)
            {
                items.Add(CreateTimeValue(time.High));
            }
            if (time.Center != null)
            {
                items.Add(CreateUsRealDateTime(time.Center));
            }
            intervalTime.Items = items.ToArray();
            return intervalTime;
        }

        private IVXB_TS CreateTimeValue(UsRealDateAndTime time)
        {
            TS ts = CreateUsRealDateTime(time);
            var result = new IVXB_TS {inclusive = time.Inclusive, value = ts.value};
            return result;
        }


        private POCD_MT000040AssociatedEntity CreateAssociatedEntity(AssociatedEntity associatedEntity)
        {
            var aEnt = new POCD_MT000040AssociatedEntity();
            //CONF:10006
            if (aEnt.associatedPerson == null && aEnt.scopingOrganization == null)
            {
                throw new Exception(
                    "Such participants, if present, SHALL have an associatedPerson or scopingOrganization element under participant/associatedEntity (CONF:10006). ");
            }
            if (associatedEntity.AssociatedPerson != null)
            {
                aEnt.associatedPerson = CreateAssociatedPerson(associatedEntity.AssociatedPerson);
                aEnt.classCode = associatedEntity.ClassCodeIfInd.Code;
            }
            if (associatedEntity.ScopingOrganization != null)
            {
                aEnt.scopingOrganization = CreateOrganization(associatedEntity.ScopingOrganization);
            }

            if (associatedEntity.Addresses != null && associatedEntity.Addresses.Count > 0)
            {
                aEnt.addr = CreateUsRealmAddresses(associatedEntity.Addresses);
            }
            if (associatedEntity.Telecoms != null && associatedEntity.Telecoms.Count > 0)
            {
                aEnt.telecom = CreateTelecoms(associatedEntity.Telecoms);
            }

            return aEnt;
        }

        private POCD_MT000040Person CreateAssociatedPerson(UsRealPersonName associatedPerson)
        {
            var person = new POCD_MT000040Person {name = new[] {CreateRealmPersonName(associatedPerson)}};
            return person;
        }

        private POCD_MT000040Authenticator[] CreateAuthenticators(List<Authenticator> authenticators)
        {
            var auth = new POCD_MT000040Authenticator[authenticators.Count];
            for (int index = 0; index < authenticators.Count; index++)
            {
                Authenticator authenticator = authenticators[index];
                auth[index] = CreateAuthenticator(authenticator);
            }
            return auth;
        }

        private POCD_MT000040Authenticator CreateAuthenticator(Authenticator authenticator)
        {
            var auth = new POCD_MT000040Authenticator();
            //CONF:5608
            if (authenticator.Time == null)
            {
                throw new Exception("The authenticator, if present, SHALL contain exactly one [1..1] time(CONF:5608).");
            }
            auth.time = CreateUsRealDateTime(authenticator.Time);
            //CONF:5610
            auth.signatureCode = CreateCodeSet(authenticator.SignatureCode);
            //CONF:5612
            if (authenticator.AssignedEntity == null)
            {
                throw new Exception(
                    "The authenticator, if present, SHALL contain exactly one [1..1] assignedEntity (CONF:5612).");
            }
            auth.assignedEntity = CreateAssignedEntity(authenticator.AssignedEntity);

            return auth;
        }

        private POCD_MT000040LegalAuthenticator CreateLegalAuthenticator(LegalAuthenticator legalAuthenticator)
        {
            var lAuth = new POCD_MT000040LegalAuthenticator();
            //CONF:5580
            if (legalAuthenticator.Time == null)
            {
                throw new Exception(
                    "The legalAuthenticator, if present, SHALL contain exactly one [1..1] time (CONF:5580).");
            }
            lAuth.time = CreateUsRealDateTime(legalAuthenticator.Time);
            lAuth.signatureCode = CreateCodeSet(legalAuthenticator.SignatureCode);

            //CONF:5585
            if (legalAuthenticator.AssignedEntity == null)
            {
                throw new Exception(
                    "The legalAuthenticator, if present, SHALL contain exactly one [1..1] assignedEntity (CONF:5585).");
            }
            lAuth.assignedEntity = CreateAssignedEntity(legalAuthenticator.AssignedEntity);
            return lAuth;
        }

        private CS CreateCodeSet(CodeSet signatureCode)
        {
            var cs = new CS
                {
                    code = signatureCode.Code,
                    codeSystem = signatureCode.CodeSystem,
                    codeSystemName = signatureCode.CodeSystemName
                };
            return cs;
        }

        private POCD_MT000040InformationRecipient[] CreateInformationRecipients(
            List<InformationRecipient> informationRecipients)
        {
            var result = new POCD_MT000040InformationRecipient[informationRecipients.Count];
            for (int index = 0; index < informationRecipients.Count; index++)
            {
                InformationRecipient informationRecipient = informationRecipients[index];
                result[index] = CreateInformationRecipient(informationRecipient);
            }

            return result;
        }

        private POCD_MT000040InformationRecipient CreateInformationRecipient(InformationRecipient informationRecipient)
        {
            var infRec = new POCD_MT000040InformationRecipient();
            //CONF:5566
            if (informationRecipient.IntendedRecipient == null)
            {
                throw new Exception(
                    "The informationRecipient, if present, SHALL contain exactly one [1..1] intendedRecipient (CONF:5566).");
            }
            //CONF:5567
            infRec.intendedRecipient = CreateIntendedRecipient(informationRecipient.IntendedRecipient);
            return infRec;
        }

        private POCD_MT000040IntendedRecipient CreateIntendedRecipient(IntendedRecipient intendedRecipient)
        {
            if (intendedRecipient.InformationRecipient == null)
            {
                throw new Exception(
                    "The informationRecipient, if present, SHALL contain at least one [1..*] name (CONF:5568).");
            }
            var intRec = new POCD_MT000040IntendedRecipient();
            //CONF:5568
            intRec.informationRecipient = CreateInformationRecipientPerson(intendedRecipient.InformationRecipient);
            //CONF:5577
            if (intendedRecipient.Organization != null)
            {
                intRec.receivedOrganization = CreateOrganization(intendedRecipient.Organization);
            }
            return intRec;
        }

        private POCD_MT000040Organization CreateOrganization(Organization organization)
        {
            var result = new POCD_MT000040Organization {name = new[] {new ON {Text = new[] {organization.Name.Family}}}};
            return result;
        }

        private POCD_MT000040Person CreateInformationRecipientPerson(UsRealPersonName informationRecipient)
        {
            var result = new POCD_MT000040Person();
            PN name = CreateRealmPersonName(informationRecipient);
            result.name = new[] {name};
            return result;
        }

        private POCD_MT000040Custodian CreateCustodian(Custodian custodian)
        {
            var cst = new POCD_MT000040Custodian();
            //CONF:5520
            if (custodian.AssignedCustodian == null)
            {
                throw new Exception("This custodian SHALL contain exactly one [1..1] assignedCustodian(CONF:5520).");
            }
            cst.assignedCustodian = CreateAssignedCustodian(custodian.AssignedCustodian);
            return cst;
        }

        private POCD_MT000040AssignedCustodian CreateAssignedCustodian(AssignedCustodian assignedCustodian)
        {
            //CONF:5521
            if (assignedCustodian.RepresentedCustodianOrganization == null)
            {
                throw new Exception(
                    "This assignedCustodian SHALL contain exactly one [1..1] representedCustodianOrganization (CONF:5521). ");
            }
            var asCst = new POCD_MT000040AssignedCustodian
                {
                    representedCustodianOrganization =
                        CreateRepresentedCustodianOrganization(assignedCustodian.RepresentedCustodianOrganization)
                };
            return asCst;
        }

        private POCD_MT000040CustodianOrganization CreateRepresentedCustodianOrganization(
            RepresentedCustodianOrganization representedCustodianOrganization)
        {
            //CONF:5522
            if (representedCustodianOrganization.NationalProviderIdentification == null)
            {
                throw new Exception(
                    "This representedCustodianOrganization SHALL contain at least one [1..*] id (CONF:5522).");
            }
            //CONF:16822
            var custOrg = new POCD_MT000040CustodianOrganization();
            custOrg.id = new II[representedCustodianOrganization.IdCount];
            for (int index = 0; index < representedCustodianOrganization.IdCount; ++index)
            {
                custOrg.id[index] = new II
                    {
                        root = NationalProviderIdentifierIdRoot,
                        extension = representedCustodianOrganization.NationalProviderIdentification
                    };
            }
            //CONF:5524
            if (string.IsNullOrEmpty(representedCustodianOrganization.Name))
            {
                throw new Exception(
                    "This representedCustodianOrganization SHALL contain exactly one [1..1] name (CONF:5524).");
            }
            custOrg.name = new ON {Text = new[] {representedCustodianOrganization.Name}};

            //CONF:5525
            if (representedCustodianOrganization.Telecom == null)
            {
                throw new Exception(
                    "This representedCustodianOrganization SHALL contain exactly one [1..1] telecom (CONF:5525).");
            }
            custOrg.telecom = CreateTelecom(representedCustodianOrganization.Telecom);

            //CONF:5559
            if (representedCustodianOrganization.Address == null)
            {
                throw new Exception(
                    "This representedCustodianOrganization SHALL contain at least one [1..*] addr (CONF:5559).");
            }
            custOrg.addr = GetAddress(representedCustodianOrganization.Address);

            return custOrg;
        }


        private POCD_MT000040Informant12[] CreateInformants(List<Informant> informants)
        {
            var result = new POCD_MT000040Informant12[informants.Count];
            for (int i = 0; i < informants.Count; i++)
            {
                result[i] = CreateInformant(informants[i]);
            }

            return result;
        }

        private POCD_MT000040Informant12 CreateInformant(Informant informant)
        {
            //CONF:8002
            if (informant.AssignedEntity != null && informant.RelatedEntity != null)
            {
                throw new Exception(
                    "SHALL contain exactly one [1..1] assignedEntity OR exactly one [1..1] relatedEntity (CONF:8002).");
            }
            if (informant.AssignedEntity == null && informant.RelatedEntity == null)
            {
                throw new Exception(
                    "SHALL contain exactly one [1..1] assignedEntity OR exactly one [1..1] relatedEntity (CONF:8002).");
            }

            var inf = new POCD_MT000040Informant12();
            if (informant.AssignedEntity != null && informant.RelatedEntity == null)
            {
                inf.Item = CreateAssignedEntityForInformant(informant.AssignedEntity);
            }
            if (informant.RelatedEntity != null && informant.AssignedEntity == null)
            {
                inf.Item = CreateRelatedEntity(informant.RelatedEntity);
            }

            return inf;
        }

        private POCD_MT000040AssignedEntity CreateAssignedEntityForInformant(AssignedEntity assignedEntity)
        {
            //(CONF:9945)
            var result = new POCD_MT000040AssignedEntity {id = new II[assignedEntity.IdCount]};
            for (int index = 0; index < assignedEntity.IdCount; ++index)
            {
                //Such ids SHOULD contain zero or one [0..1] @root="2.16.840.1.113883.4.6" National Provider Identifier (CONF:16821).
                //CONF:9946
                result.id[index] = new II
                    {
                        root = NationalProviderIdentifierIdRoot,
                        extension = assignedEntity.NationalProviderIdentification
                    };
                index++;
            }
            //(CONF:8220).
            if (assignedEntity.Addresses != null && assignedEntity.Addresses.Count > 0)
            {
                result.addr = CreateUsRealmAddresses(assignedEntity.Addresses);
            }

            //(CONF:8221).
            if (assignedEntity.AssignedPerson == null)
            {
                throw new Exception(
                    "SHALL contain exactly one [1..1] assignedPerson OR exactly one [1..1] relatedPerson (CONF:8221).");
            }
            result.assignedPerson = CreateAssignedPerson(assignedEntity.AssignedPerson);
            //CONF:9947
            if (assignedEntity.NuccHealthCareProviderTaxonomyCode != null)
            {
                result.code = CreateCodeValueSet(assignedEntity.NuccHealthCareProviderTaxonomyCode);
            }
            return result;
        }

        private POCD_MT000040RelatedEntity CreateRelatedEntity(RelatedEntity relatedEntity)
        {
            var result = new POCD_MT000040RelatedEntity();
            //(CONF:8220).
            if (relatedEntity.Addresses != null && relatedEntity.Addresses.Count > 0)
            {
                result.addr = CreateUsRealmAddresses(relatedEntity.Addresses);
            }

            //(CONF:8221).
            if (relatedEntity.RelatedPerson == null)
            {
                throw new Exception(
                    "SHALL contain exactly one [1..1] assignedPerson OR exactly one [1..1] relatedPerson (CONF:8221).");
            }
            result.relatedPerson = CreateRelatedPerson(relatedEntity.RelatedPerson);
            //CONF:9947
            if (relatedEntity.NuccHealthCareProviderTaxonomyCode != null)
            {
                result.code = CreateCodeValueSet(relatedEntity.NuccHealthCareProviderTaxonomyCode);
            }


            return result;
        }

        private POCD_MT000040Person CreateRelatedPerson(RelatedPerson relatedPerson)
        {
            var relPerson = new POCD_MT000040Person();
            //CONF:8222
            if (relatedPerson.Names == null || relatedPerson.Names.Count == 0)
            {
                throw new Exception(
                    "The relatedPerson, SHALL contain at least one [1..*] name (CONF:8222).");
            }
            List<UsRealPersonName> names = relatedPerson.Names;
            relPerson.name = new PN[names.Count];
            int index = 0;
            foreach (UsRealPersonName name in names)
            {
                relPerson.name[index] = CreateRealmPersonName(name);
                index++;
            }
            return relPerson;
        }

        private POCD_MT000040DataEnterer CreateDataEnterer(DataEnterer dataEnterer)
        {
            var result = new POCD_MT000040DataEnterer();
            //(CONF:5442).
            if (dataEnterer.AssignedEntity == null)
            {
                throw new Exception(
                    "The dataEnterer, if present, SHALL contain exactly one [1..1] assignedEntity (CONF:5442).");
            }
            result.assignedEntity = CreateAssignedEntity(dataEnterer.AssignedEntity);
            return result;
        }

        private POCD_MT000040AssignedEntity CreateAssignedEntity(AssignedEntity assignedEntity,
                                                                 bool allowEmptyIds = false,
                                                                 bool allowEmptyAddresses = false,
                                                                 bool allowEmptyTelecoms = false,
                                                                 bool allowNoAssignedPerson = false)
        {
            var result = new POCD_MT000040AssignedEntity();
            //(CONF:5443)
            if (assignedEntity.NationalProviderIdentification == null)
            {
                throw new Exception("This assignedEntity SHALL contain at least one [1..*] id (CONF:5443).");
            }
            result.id = new II[assignedEntity.IdCount];
            for (int index = 0; index < assignedEntity.IdCount; ++index)
            {
                //Such ids SHOULD contain zero or one [0..1] @root="2.16.840.1.113883.4.6" National Provider Identifier (CONF:16821).
                //CONF:16786
                result.id[index] = new II
                    {
                        root = NationalProviderIdentifierIdRoot,
                        extension = assignedEntity.NationalProviderIdentification
                    };
                index++;
            }
            //(CONF:5460).
            if ((assignedEntity.Addresses == null || assignedEntity.Addresses.Count == 0) && !allowEmptyAddresses)
            {
                throw new Exception("This assignedEntity SHALL contain at least one [1..*] addr (CONF:5460).");
            }

            result.addr = CreateUsRealmAddresses(assignedEntity.Addresses);
            //(CONF:5466).
            if ((assignedEntity.Telecoms == null || assignedEntity.Telecoms.Count == 0) && !allowEmptyTelecoms)
            {
                throw new Exception("This assignedEntity SHALL contain at least one [1..*] telecom (CONF:5466).");
            }
            result.telecom = CreateTelecoms(assignedEntity.Telecoms);

            //(CONF:5469).
            if (assignedEntity.AssignedPerson == null && !allowNoAssignedPerson)
            {
                throw new Exception("This assignedEntity SHALL contain exactly one [1..1] assignedPerson (CONF:5469).");
            }
            result.assignedPerson = CreateAssignedPerson(assignedEntity.AssignedPerson);


            if (assignedEntity.NuccHealthCareProviderTaxonomyCode != null)
            {
                result.code = CreateCodeValueSet(assignedEntity.NuccHealthCareProviderTaxonomyCode);
            }
            return result;
        }

        private POCD_MT000040Person CreateAssignedPerson(AssignedPerson aPerson)
        {
            var assignedPerson = new POCD_MT000040Person();
            //CONF:5470
            if (aPerson.Names == null || aPerson.Names.Count == 0)
            {
                throw new Exception(
                    "The assignedPerson, if present, SHALL contain at least one [1..*] name (CONF:5470).");
            }
            List<UsRealPersonName> names = aPerson.Names;
            assignedPerson.name = new PN[names.Count];
            int index = 0;
            foreach (UsRealPersonName name in names)
            {
                assignedPerson.name[index] = CreateRealmPersonName(name);
                index++;
            }
            return assignedPerson;
        }

        private TEL[] CreateTelecoms(List<Telecom> telecoms)
        {
            var tel = new TEL[telecoms.Count];
            for (int i = 0; i < telecoms.Count; i++)
            {
                tel[i] = CreateTelecom(telecoms[i]);
            }
            return tel;
        }

        private AD[] CreateUsRealmAddresses(List<UsRealmAddress> addresses)
        {
            var ad = new AD[addresses.Count];
            for (int index = 0; index < addresses.Count; index++)
            {
                UsRealmAddress usRealmAddress = addresses[index];
                ad[index] = GetAddress(usRealmAddress);
            }
            return ad;
        }

        private POCD_MT000040Author CreateAuthor(Author author)
        {
            var cdaAuth = new POCD_MT000040Author();
            //(CONF:5445).
            if (author.UsRealDateAndTime == null)
            {
                throw new Exception("Such authors SHALL contain exactly one [1..1] time (CONF:5445).");
            }
            cdaAuth.time = CreateUsRealDateTime(author.UsRealDateAndTime);
            //CONF:5448
            if (author.AssignedAuthor == null)
            {
                throw new Exception("Such authors SHALL contain exactly one [1..1] assignedAuthor (CONF:5448).");
            }
            cdaAuth.assignedAuthor = CreateAssignedAuthor(author.AssignedAuthor);
            return cdaAuth;
        }

        private POCD_MT000040AssignedAuthor CreateAssignedAuthor(AssignedAuthor assignedAuthor)
        {
            if (assignedAuthor.NationalProviderIdentification == null)
            {
                throw new Exception(
                    "1.	SHALL contain exactly one [1..1] @root=\"2.16.840.1.113883.4.6\" National Provider Identifie(CONF:16786).");
            }

            var cdaAssAuthor = new POCD_MT000040AssignedAuthor
                {
                    //CONF:16786
                    id =
                        new[]
                            {
                                new II
                                    {
                                        root = NationalProviderIdentifierIdRoot,
                                        extension = assignedAuthor.NationalProviderIdentification
                                    }
                            }
                };

            //CONF:16787
            if (assignedAuthor.Code != null)
            {
                cdaAssAuthor.code = CreateCodeValueSet(assignedAuthor.Code);
            }
            //CONF:5452
            if (assignedAuthor.Addresses == null || assignedAuthor.Addresses.Count == 0)
            {
                throw new Exception("This assignedAuthor SHALL contain at least one [1..*] addr(CONF:5452).");
            }
            cdaAssAuthor.addr = CreateUsRealmAddresses(assignedAuthor.Addresses);
            //CONF:5428
            if (assignedAuthor.Telecoms == null || assignedAuthor.Telecoms.Count == 0)
            {
                throw new Exception("This assignedAuthor SHALL contain at least one [1..*] telecom(CONF:5428).");
            }
            cdaAssAuthor.telecom = CreateTelecoms(assignedAuthor.Telecoms);

            //CONF:5430
            object item = null;
            if (assignedAuthor.AssignedPerson != null)
            {
                var assignedPerson = new POCD_MT000040Person();
                //CONF:16789
                if (assignedAuthor.AssignedPerson.Names == null || assignedAuthor.AssignedPerson.Names.Count == 0)
                {
                    throw new Exception(
                        "The assignedPerson, if present, SHALL contain at least one [1..*] name (CONF:16789).");
                }
                List<UsRealPersonName> names = assignedAuthor.AssignedPerson.Names;
                assignedPerson.name = new PN[names.Count];
                int index = 0;
                foreach (UsRealPersonName name in names)
                {
                    assignedPerson.name[index] = CreateRealmPersonName(name);
                    index++;
                }
                item = assignedPerson;
            }
            else if (assignedAuthor.AssignedAuthoringDevice != null)
            {
                //CONF:16784
                var assignedAuthDevice = new POCD_MT000040AuthoringDevice();
                if (string.IsNullOrEmpty(assignedAuthor.AssignedAuthoringDevice.ManufacturerModelName))
                {
                    throw new Exception(
                        "The assignedAuthoringDevice, if present, SHALL contain exactly one [1..1] manufacturerModelName (CONF:16784).");
                }
                assignedAuthDevice.manufacturerModelName = new SC
                    {
                        Text = new[] {assignedAuthor.AssignedAuthoringDevice.ManufacturerModelName}
                    };
                //CONF:16785
                if (string.IsNullOrEmpty(assignedAuthor.AssignedAuthoringDevice.SoftwareName))
                {
                    throw new Exception(
                        "The assignedAuthoringDevice, if present, SHALL contain exactly one [1..1] softwareName (CONF:16785). ");
                }
                assignedAuthDevice.softwareName = new SC
                    {
                        Text = new[] {assignedAuthor.AssignedAuthoringDevice.SoftwareName}
                    };
            }
            //CONF:16790
            if ((assignedAuthor.AssignedPerson == null && assignedAuthor.AssignedAuthoringDevice == null) ||
                (assignedAuthor.AssignedPerson != null && assignedAuthor.AssignedAuthoringDevice != null))
            {
                throw new Exception(
                    " There SHALL be exactly one assignedAuthor/assignedPerson or exactly one assignedAuthor/assignedAuthoringDevice (CONF:16790).");
            }
            cdaAssAuthor.Item = item;
            return cdaAssAuthor;
        }

        private TS CreateUsRealDateTime(UsRealDateAndTime usRealDateAndTime)
        {
            var result = new TS();
            string dateFormatPattern = DateTimeFormatToDay;
            //(CONF:10078).

            if (usRealDateAndTime.Year == 0 || usRealDateAndTime.Month == 0 || usRealDateAndTime.Day == 0)
            {
                throw new Exception("SHALL be precise to the day (CONF:10078).");
            }

            var dt = new DateTime(usRealDateAndTime.Year, usRealDateAndTime.Month,
                                  usRealDateAndTime.Day);
            //CONF:10079
            if (usRealDateAndTime.Minute > 0)
            {
                dt = dt.AddMinutes(usRealDateAndTime.Minute);
                dateFormatPattern = DateTimeFormatToMinute;
            }
            //CONF:10080
            if (usRealDateAndTime.Second > 0)
            {
                dt = dt.AddSeconds(usRealDateAndTime.Second);
                dateFormatPattern = DateTimeFormatToSecond;
            }

            //CONF:10081
            if (usRealDateAndTime.Minute > 0 && !string.IsNullOrEmpty(usRealDateAndTime.TimeZoneOffset))
            {
                dateFormatPattern = dateFormatPattern + usRealDateAndTime.TimeZoneOffset;
            }
            result.value = dt.ToString(dateFormatPattern);
            return result;
        }

        private void AddPatientRole(POCD_MT000040RecordTarget cdaRecordTarget, RecordTarget recordTarget)
        {
            var patientRole = new POCD_MT000040PatientRole();
            cdaRecordTarget.patientRole = patientRole;
            //CONF:5268
            if (recordTarget.MedicalHicNumberPatientRoleIds == null)
            {
                throw new Exception("This patientRole SHALL contain at least one [1..1] id (CONF:5268).");
            }
            patientRole.id = new II[1];
            patientRole.id[0] = new II
                {
                    root = PatientRoleDefaultIdRoot,
                    extension = recordTarget.MedicalHicNumberPatientRoleIds
                };
            UsRealmAddress[] addresses = recordTarget.Addresses;
            //CONF:5271
            if (addresses.Length < 1)
                throw new Exception("This patientRole SHALL contain at least one [1..*] addr (CONF:5271).");
            AddPatientRoleAddresses(patientRole, addresses);

            Telecom[] telecoms = recordTarget.Telecoms;
            //CONF:5280
            if (telecoms.Length < 1)
                throw new Exception("This patientRole SHALL contain at least one [1..*] telecom(CONF:5280).");
            AddPatientRoleTelecom(patientRole, telecoms);
            Patient patient = recordTarget.Patient;
            //(CONF:5283)
            if (patient == null)
                throw new Exception("This patientRole SHALL contain exactly one [1..1] patient (CONF:5283).");
            AddPatientRolePatient(patientRole, patient);
        }

        private void AddPatientRolePatient(POCD_MT000040PatientRole patientRole, Patient patient)
        {
            patientRole.patient = new POCD_MT000040Patient();
            //CONF:5284
            if (patient.Name == null)
            {
                throw new Exception("This patient SHALL contain exactly one [1..1] name(CONF:5284).");
            }

            PN name = CreateRealmPersonName(patient.Name);

            patientRole.patient.name = new[] {name};

            //CONF:6394
            if (patient.Gender == null)
            {
                throw new Exception(
                    "This patient SHALL contain exactly one [1..1] administrativeGenderCode, which SHALL be selected from ValueSet Administrative Gender (HL7 V3) 2.16.840.1.113883.1.11.1 DYNAMIC (CONF:6394).");
            }
            patientRole.patient.administrativeGenderCode = CreateCodeValueSet(patient.Gender);

            //CONF:5298

            BirthTime birthT = patient.BirthTime;
            if (birthT == null)
            {
                throw new Exception("This patient SHALL contain exactly one [1..1] birthTime(CONF:5298).");
            }
            if (string.IsNullOrEmpty(birthT.Year))
            {
                throw new Exception("Birth time SHALL be precise to year (CONF:5299).");
            }
            string bTime = birthT.Year + birthT.Month + birthT.Day;
            var birthTime = new TS {value = bTime};
            patientRole.patient.birthTime = birthTime;

            //CONF:5303
            if (patient.MaritalStatusCode != null)
            {
                patientRole.patient.maritalStatusCode = CreateCodeValueSet(patient.MaritalStatusCode);
            }
            //CONF:5317
            if (patient.ReligiousAffiliation != null)
            {
                patientRole.patient.religiousAffiliationCode = CreateCodeValueSet(patient.ReligiousAffiliation);
            }
            //CONF:5322
            if (patient.MainRaceCode != null)
            {
                patientRole.patient.raceCode = CreateCodeValueSet(patient.MainRaceCode);
            }
            //CONF:7263
            if (patient.RaceCodes != null && patient.RaceCodes.Count > 0)
            {
                patientRole.patient.raceCode1 = new CE[patient.RaceCodes.Count];
                int index = 0;
                foreach (CodeSet race in patient.RaceCodes)
                {
                    patientRole.patient.raceCode1[index] = CreateCodeValueSet(race);
                    index++;
                }
            }
            //CONF:5323
            if (patient.EthnicGroupCode != null)
            {
                patientRole.patient.ethnicGroupCode = CreateCodeValueSet(patient.EthnicGroupCode);
            }
            //(CONF:5325)
            if (patient.Guardians != null && patient.Guardians.Count > 0)
            {
                //(CONF:5326)
                List<Guardian> guardians = patient.Guardians;
                patientRole.patient.guardian = new POCD_MT000040Guardian[guardians.Count];
                int index = 0;
                foreach (Guardian guardian in guardians)
                {
                    patientRole.patient.guardian[index] = CreateGuardian(guardian);
                    index++;
                }
            }

            //CONF:5395
            if (patient.BirthPlace != null)
            {
                //CONF:5396
                if (patient.BirthPlace.Place == null)
                    throw new Exception(
                        "The birthplace, if present, SHALL contain exactly one [1..1] place (CONF:5396).");
                Address addr = patient.BirthPlace.Place.Address;
                //CONF:5397
                if (addr == null)
                {
                    throw new Exception("This place SHALL contain exactly one [1..1] addr(CONF:5397).");
                }
                patientRole.patient.birthplace = new POCD_MT000040Birthplace
                    {
                        place = new POCD_MT000040Place {addr = GetAddressNonReal(addr)}
                    };
            }

            //CONF:5406
            if (patient.LanguageCommunications != null && patient.LanguageCommunications.Count > 0)
            {
                List<LanguageCommunication> lcoms = patient.LanguageCommunications;
                patientRole.patient.languageCommunication = new POCD_MT000040LanguageCommunication[lcoms.Count];
                int index = 0;
                foreach (LanguageCommunication languageCommunication in lcoms)
                {
                    patientRole.patient.languageCommunication[index] = CreateLanguageCommunication(languageCommunication);
                    index++;
                }
            }

            //CONF:5416
            if (patient.ProviderOrganization != null)
            {
                patientRole.providerOrganization = CreateProviderOrganization(patient.ProviderOrganization);
            }
        }

        private POCD_MT000040Organization CreateProviderOrganization(ProviderOrganization providerOrganization)
        {
            var porg = new POCD_MT000040Organization();
            //CONF:5417
            if (providerOrganization.Ids == null || providerOrganization.Ids.Count == 0)
            {
                throw new Exception(
                    "The providerOrganization, if present, SHALL contain at least one [1..*] id (CONF:5417). ");
            }
            List<string> ids = providerOrganization.Ids;
            porg.id = new II[ids.Count];
            int index = 0;
            foreach (string id in ids)
            {
                porg.id[index] = new II {root = id};
                index++;
            }

            //CONF:5419
            if (providerOrganization.OrganizationNames == null || providerOrganization.OrganizationNames.Count == 0)
            {
                throw new Exception(
                    "The providerOrganization, if present, SHALL contain at least one [1..*] name (CONF:5419).");
            }

            porg.name = new ON[providerOrganization.OrganizationNames.Count];
            List<OrganizationName> orgNames = providerOrganization.OrganizationNames;
            index = 0;
            foreach (OrganizationName organizationName in orgNames)
            {
                porg.name[index] = CreateOrganizationName(organizationName);
                index++;
            }
            //CONF:5420
            if (providerOrganization.Telecoms == null || providerOrganization.Telecoms.Count == 0)
            {
                throw new Exception(
                    "The providerOrganization, if present, SHALL contain at least one [1..*] telecom (CONF:5420).");
            }
            index = 0;
            List<Telecom> telecoms = providerOrganization.Telecoms;
            porg.telecom = new TEL[telecoms.Count];
            foreach (Telecom telecom in telecoms)
            {
                porg.telecom[index] = CreateTelecom(telecom);
                index++;
            }
            //CONF:5422
            if (providerOrganization.Addresses == null || providerOrganization.Addresses.Count == 0)
            {
                throw new Exception(
                    "The providerOrganization, if present, SHALL contain at least one [1..*] addr (CONF:5422).");
            }
            index = 0;
            List<UsRealmAddress> addrs = providerOrganization.Addresses;
            porg.addr = new AD[addrs.Count];
            foreach (UsRealmAddress usRealmAddress in addrs)
            {
                porg.addr[index] = GetAddress(usRealmAddress);
                index++;
            }

            return porg;
        }

        private ON CreateOrganizationName(OrganizationName organizationName)
        {
            var on = new ON();
            if (!string.IsNullOrEmpty(organizationName.Name))
            {
                on.Text = new[] {organizationName.Name};
            }
            return on;
        }

        private POCD_MT000040LanguageCommunication CreateLanguageCommunication(
            LanguageCommunication languageCommunication)
        {
            //CONF:5407
            if (languageCommunication.LanguageCode == null)
                throw new Exception(
                    "The languageCommunication, if present, SHALL contain exactly one [1..1] languageCode, which SHALL be selected from ValueSet Language2.16.840.1.113883.1.11.11526 DYNAMIC(CONF:5407).");

            var lcom = new POCD_MT000040LanguageCommunication
                {
                    languageCode = new CS
                        {
                            code = languageCommunication.LanguageCode.Code,
                            codeSystemName = languageCommunication.LanguageCode.CodeSystemName,
                            codeSystem = languageCommunication.LanguageCode.CodeSystem
                        }
                };
            //CONF:5409
            if (languageCommunication.ModeCode != null)
            {
                lcom.modeCode = CreateCodeValueSet(languageCommunication.ModeCode);
            }
            //CONF:5414
            if (languageCommunication.ProficiencyLevelCode != null)
            {
                lcom.proficiencyLevelCode = CreateCodeValueSet(languageCommunication.ProficiencyLevelCode);
            }
            //CONF:5414
            if (languageCommunication.PreferenceInd)
            {
                lcom.preferenceInd = new BL {value = languageCommunication.PreferenceInd, valueSpecified = true};
            }

            return lcom;
        }

        private AD GetAddressNonReal(Address addr)
        {
            var ad = new AD();
            var items = new List<ADXP>();
            if (addr.Country != null)
            {
                //CONF:5404
                var country = new adxpcountry {Text = new[] {addr.Country.Code}};
                items.Add(country);
                if (addr.Country.Code.Equals("US") && addr.State == null)
                {
                    //(CONF:5402).
                    throw new Exception(
                        "If country is US, this addr SHALL contain exactly one [1..1] state, which SHALL be selected from ValueSet 2.16.840.1.113883.3.88.12.80.1 StateValueSet DYNAMIC (CONF:5402).");
                }
                //CONF:5402
                if (addr.Country.Code.Equals("US"))
                {
                    var state = new adxpstate {Text = new[] {addr.State.Code}};
                    items.Add(state);
                }
            }
            //CONF:5403
            if (addr.PostalCode != null)
            {
                var postalCode = new adxppostalCode {Text = new[] {addr.PostalCode.Code}};
                items.Add(postalCode);
            }

            ad.Items = items.ToArray();
            return ad;
        }

        private PN CreateRealmPersonName(UsRealPersonName pName)
        {
            var name = new PN();
            var namesItems = new List<ENXP>();

            //CONF:7159
            if (string.IsNullOrEmpty(pName.Family))
            {
                throw new Exception("SHALL contain exactly one [1..1] family (CONF:7159)");
            }
            namesItems.Add(new enfamily {Text = new[] {pName.Family}});
            //CONF:7157
            if (pName.Given == null || pName.Given.Length < 1)
            {
                throw new Exception("SHALL contain at least one [1..*] given (CONF:7157)");
            }
            string[] givenNames = pName.Given;
            namesItems.AddRange(givenNames.Select(gName => new engiven {Text = new[] {gName}}).Cast<ENXP>());
            name.Items = namesItems.ToArray();
            return name;
        }

        private POCD_MT000040Guardian CreateGuardian(Guardian guardian)
        {
            var g = new POCD_MT000040Guardian();
            //CONF:5326
            g.code = CreateCodeValueSet(guardian.PersonalRelationshipRoleType);
            //CONF:5359
            int index = 0;
            if (guardian.Addresses != null && guardian.Addresses.Count > 0)
            {
                g.addr = new AD[guardian.Addresses.Count];
                index = 0;
                List<UsRealmAddress> address = guardian.Addresses;
                foreach (UsRealmAddress usRealmAddress in address)
                {
                    g.addr[index] = GetAddress(usRealmAddress);
                    index++;
                }
            }
            //CONF:5382
            if (guardian.Telecoms != null && guardian.Telecoms.Count > 0)
            {
                g.telecom = new TEL[guardian.Telecoms.Count];
                index = 0;
                List<Telecom> telecoms = guardian.Telecoms;
                foreach (Telecom telecom in telecoms)
                {
                    g.telecom[index] = CreateTelecom(telecom);
                }
            }
            //CONF:5385
            if (guardian.GuardianPerson == null)
            {
                throw new Exception(
                    "The guardian, if present, SHALL contain exactly one [1..1] guardianPerson (CONF:5385).");
            }

            //CONF:5386
            GuardianPerson guardianP = guardian.GuardianPerson;
            if (guardianP.Names == null || guardianP.Names.Count == 0)
            {
                throw new Exception("This guardianPerson SHALL contain at least one [1..*] name (CONF:5386)");
            }

            var guardianPerson = new POCD_MT000040Person();
            guardianPerson.name = new PN[guardianP.Names.Count];
            List<UsRealPersonName> names = guardianP.Names;
            index = 0;
            foreach (UsRealPersonName usRealPatientName in names)
            {
                guardianPerson.name[index] = CreateRealmPersonName(usRealPatientName);
                index++;
            }

            return g;
        }

        private CE CreateCodeValueSet(CodeSet vs)
        {
            return new CE
                {
                    code = vs.Code,
                    codeSystem = vs.CodeSystem,
                    displayName = vs.PrintName,
                    codeSystemName = vs.CodeSystemName
                };
        }

        private void AddPatientRoleTelecom(POCD_MT000040PatientRole patientRole, Telecom[] telecoms)
        {
            patientRole.telecom = new TEL[telecoms.Length];
            for (int index = 0; index < telecoms.Length; index++)
            {
                Telecom telecom = telecoms[index];
                patientRole.telecom[index] = CreateTelecom(telecom);
            }
        }

        private TEL CreateTelecom(Telecom telecom)
        {
            if (telecom.Use == null || telecom.Use.Code == null)
            {
                throw new Exception(
                    "The telecom, if present, SHOULD contain exactly  one [1..1] @use, which SHALL be selected from ValueSet Telecom Use (US Realm Header) 2.16.840.1.113883.11.20.9.20 DYNAMIC (CONF:7993).");
            }

            return new TEL
                {
                    use = new[] {telecom.Use.Code},
                    value = telecom.Value
                };
        }

        private void AddPatientRoleAddresses(POCD_MT000040PatientRole patientRole, UsRealmAddress[] addresses)
        {
            patientRole.addr = new AD[addresses.Length];
            for (int index = 0; index < addresses.Length; index++)
            {
                AD ad = GetAddress(addresses[index]);
                patientRole.addr[index] = ad;
            }
        }

        private AD GetAddress(UsRealmAddress usRealmAddress)
        {
            var adxpList = new List<ADXP>();
            //CONF:7291
            if (usRealmAddress.StreetAddressLine.Length < 1)
                throw new Exception(
                    "SHALL contain at least one and not more than 4 streetAddressLine (CONF:7291)");
            var ad = new AD();
            //CONF:7295
            string country = usRealmAddress.Country;
            var countryMapped = new adxpcountry {Text = new[] {country}};
            AddIfNotEmpty(country, countryMapped, adxpList);
            string city = usRealmAddress.City;

            //CONF:7292
            if (string.IsNullOrEmpty(city))
                throw new Exception("SHALL contain exactly one [1..1] city (CONF:7292).");
            var cityMapped = new adxpcity {Text = new[] {city}};
            adxpList.Add(cityMapped);

            string state = usRealmAddress.State;
            //CONF:10024
            if (string.IsNullOrEmpty(state) && country.SequenceEqual("US"))
            {
                throw new Exception(
                    "State is required if the country is US. If country is not specified, its assumed to be US. If country is something other than US, the state MAY be present but MAY be bound to different vocabularies (CONF:10024).");
            }
            //CONF:7293
            var stateMapped = new adxpstate {Text = new[] {state}};
            AddIfNotEmpty(state, stateMapped, adxpList);

            //CONF:7294
            string postalCode = usRealmAddress.PostalCode;
            var postalCodeMapped = new adxppostalCode {Text = new[] {postalCode}};
            if ((string.IsNullOrEmpty(country) || country.SequenceEqual("US")) && string.IsNullOrEmpty(postalCode))
            {
                //CONF:10025
                throw new Exception(
                    "PostalCode is required if the country is US. If country is not specified, its assumed to be US. If country is something other than US, the postalCode MAY be present but MAY be bound to different vocabularies (CONF:10025).");
            }
            AddIfNotEmpty(postalCode, postalCodeMapped, adxpList);

            string[] streetAddressLine = usRealmAddress.StreetAddressLine;
            foreach (string streetAddress in streetAddressLine)
            {
                var stAdrMt = new adxpstreetAddressLine {Text = new[] {streetAddress}};
                AddIfNotEmpty(streetAddress, stAdrMt, adxpList);
            }
            ad.Items = adxpList.ToArray();
            return ad;
        }

        private void AddIfNotEmpty(string val, ADXP adxpMapped, List<ADXP> adxpList)
        {
            if (!string.IsNullOrEmpty(val))
            {
                adxpList.Add(adxpMapped);
            }
        }
    }
}