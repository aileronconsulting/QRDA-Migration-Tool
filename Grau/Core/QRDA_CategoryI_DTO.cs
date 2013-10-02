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
using System.Text;
using System.Threading.Tasks;
using Category_I;

namespace Grau.Core
{
    public class QrdaCategoryIDto
    {
        public string CdaId { get; set; }
        public string CdaTitle { get; set; }
        public UsRealDateAndTime EffectiveTimeDate { get; set; }
        public bool EffectiveTimeHasMinutes { get; set; }
        public bool EffectiveTimeHasSeconds { get; set; }
        public CodeSet ConfidentialityCode { get; set; }
        public CodeSet LanguageCode { get; set; }
        public string CdaSetId { get; set; }
        public string VersionNumber { get; set; }
        public RecordTarget RecordTarget { get; set; }
        public List<Author> Authors { get; set; }

        public DataEnterer DataEnterer { get; set; }

        public List<Informant> Informants { get; set; }

        public Custodian Custodian { get; set; }

        public List<InformationRecipient> InformationRecipients { get; set; }

        public LegalAuthenticator LegalAuthenticator { get; set; }

        public List<Authenticator> Authenticators { get; set; }

        public List<Participant> Participants { get; set; }

        public List<InFulfillmentOf> InFulfillmentOfs { get; set; }

        public List<DocumentationOf> DocumentationOfs { get; set; }

        public List<Authorization> Authorizations { get; set; }

        public ComponentOf ComponentOf { get; set; }

        public ComponentBody Body { get; set; }
    }

    public class ComponentBody
    {
        public MeasureSection MeasureSection { get; set; }
        public ReportingParametersSection ReportingParametersSection { get; set; }
        public PatientDataSection PatientDataSection { get; set; }
    }

    public class PatientDataSection
    {

    }

    public class ReportingParametersSection
    {
    }

    public class MeasureSection
    {
    }

    public class ComponentOf
    {
        public EncompassingEncounter EncompassingEncounter { get; set; }
    }

    public class EncompassingEncounter
    {
        public List<string> Ids { get; set; }
        public IntervalTime EffectiveTime { get; set; }
    }

    public class Authorization
    {
        public Consent Consent { get; set; }
    }

    public class Consent
    {
        public List<string> Ids { get; set; }
        public CodeSet Code { get; set; }
        public string StatusCode { get { return "completed"; } }
    }

    public class DocumentationOf
    {
        public EventService ServiceEvent { get; set; }
    }

    public class EventService
    {
        public IntervalTime EffectiveTime { get; set; }

        public List<Performer> Performers { get; set; }
    }

    public class Performer
    {
        public x_ServiceEventPerformer DefaultTypeCode { get {return x_ServiceEventPerformer.PRF;}}

        public CodeSet FunctionCode { get; set; }

        public AssignedEntity AssignedEntity { get; set; }

        public RepresentedOrganization RepresentedOrganization { get; set; }
    }

    public class RepresentedOrganization
    {
        public string TaxIdNumber { get; set; }
        public string FacilityCmsCertificationNumber { get; set; }
    }

    public class InFulfillmentOf
    {
        public Order Order { get; set; }
    }

    public class Order
    {
        public List<string> Ids { get; set; } 
    }

    public class Participant
    {
        public IntervalTime Time { get; set; }
        public String TypeCode { get; set; }
        public AssociatedEntity AssociatedEntity { get; set; }
    }

    public class IntervalTime
    {
        public UsRealDateAndTime High { get; set; }
        public UsRealDateAndTime Low { get; set; }
        public UsRealDateAndTime Center { get; set; }
    }

    public class AssociatedEntity
    {
        public UsRealPersonName AssociatedPerson { get; set; }
        public CodeSet ClassCodeIfInd { get; set; }
        public List<UsRealmAddress> Addresses { get; set; }

        public Organization ScopingOrganization { get; set; }

        public List<Telecom> Telecoms { get; set; }
    }

    public class Authenticator  
    {
        public UsRealDateAndTime Time { get; set; }
        public CodeSet SignatureCode {get
        {
            return new CodeSet
                {
                    Code = "S",
                    CodeSystemName = "Participationsignature",
                    CodeSystem = "2.16.840.1.113883.5.89"
                };
        }}
        public AssignedEntity AssignedEntity { get; set; }
    }

    public class LegalAuthenticator 
    {
        public UsRealDateAndTime Time { get; set; }
        public CodeSet SignatureCode {get
        {
            return new CodeSet
                {
                    Code = "S",
                    CodeSystemName = "Participationsignature",
                    CodeSystem = "2.16.840.1.113883.5.89"
                };
        }}

        public AssignedEntity AssignedEntity { get; set; }
    }

    public class InformationRecipient
    {
        public IntendedRecipient IntendedRecipient { get; set; }
    }

    public class IntendedRecipient
    {
        public UsRealPersonName InformationRecipient { get; set; }
        public Organization Organization { get; set; }
    }

    public class Organization
    {
        public UsRealPersonName Name { get; set; }
    }

    public class Custodian
    {
        public AssignedCustodian AssignedCustodian { get; set; }

    }

    public class AssignedCustodian
    {
        public RepresentedCustodianOrganization RepresentedCustodianOrganization { get; set; }
    }

    public class RepresentedCustodianOrganization
    {
        public int IdCount { get; set; }
        public string Name { get; set; }
        public Telecom Telecom { get; set; }
        public UsRealmAddress Address { get; set; }

        public string NationalProviderIdentification { get; set; }
    }

    public class Informant
    {
        public AssignedEntity AssignedEntity { get; set; }
        public RelatedEntity RelatedEntity { get; set; }
    }

    public class RelatedEntity
    {
        public List<UsRealmAddress> Addresses { get; set; }
        public RelatedPerson RelatedPerson { get; set; }
        public CodeSet NuccHealthCareProviderTaxonomyCode { get; set; }
    }

    public class RelatedPerson
    {
        public List<UsRealPersonName> Names { get; set; } 
    }

    public class DataEnterer
    {
        public AssignedEntity AssignedEntity { get; set; }
    }

    public class AssignedEntity 
    {
        public int IdCount { get; set; }
        public CodeSet NuccHealthCareProviderTaxonomyCode { get; set; }
        public List<UsRealmAddress> Addresses { get; set; }
        public List<Telecom> Telecoms { get; set; }
        public AssignedPerson AssignedPerson { get; set; }

        public string NationalProviderIdentification { get; set; }
    }

    public class Author
    {
        public UsRealDateAndTime UsRealDateAndTime { get; set; }

        public AssignedAuthor AssignedAuthor { get; set; }
    }

    public class AssignedAuthor
    {
        public string IdRoot { get; set; }
        public CodeSet Code { get; set; }
        public List<UsRealmAddress> Addresses { get; set; }
        public List<Telecom> Telecoms { get; set; }
        public AssignedPerson AssignedPerson { get; set; }
        public AssignedAuthoringDevice AssignedAuthoringDevice { get; set; }

        public string NationalProviderIdentification { get; set; }
    }

    public class AssignedAuthoringDevice
    {
        public string ManufacturerModelName { get; set; }
        public string SoftwareName { get; set; }
    }

    public class AssignedPerson
    {
        public List<UsRealPersonName> Names { get; set; }
    }

    public class UsRealDateAndTime
    {
        //SHALL be precise to the day (CONF:10127).
        //SHOULD be precise to the minute (CONF:10128).
        //MAY be precise to the second (CONF:10129).
        //If more precise than day, SHOULD include time-zone offset (CONF:10130).
        public int Day { get; set; }
        public double Minute { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Second { get; set; }
        public string TimeZoneOffset { get; set; }
        public bool Inclusive { get; set; }
    }

    public class RecordTarget
    {
        public string MedicalHicNumberPatientRoleIds { get; set; }

        public UsRealmAddress[] Addresses { get; set; }

        public Telecom[] Telecoms { get; set; }

        public Patient Patient { get; set; }
    }

    public class Patient
    {
        public UsRealPersonName Name { get; set; }

        public CodeSet Gender { get; set; }

        public BirthTime BirthTime { get; set; }

        public CodeSet MaritalStatusCode { get; set; }

        public CodeSet ReligiousAffiliation { get; set; }

        public CodeSet MainRaceCode { get; set; }

        public List<CodeSet> RaceCodes { get; set; }

        public CodeSet EthnicGroupCode { get; set; }

        public List<Guardian> Guardians { get; set; }

        public BirthPlace BirthPlace { get; set; }

        public List<LanguageCommunication> LanguageCommunications { get; set; }

        public ProviderOrganization ProviderOrganization { get; set; }
    }

    public class ProviderOrganization
    {
        public List<string> Ids { get; set; }

        public List<OrganizationName> OrganizationNames { get; set; }

        public List<Telecom> Telecoms { get; set; }

        public List<UsRealmAddress> Addresses { get; set; }
    }

    public class OrganizationName
    {
        public string Name { get; set; }
    }

    public class LanguageCommunication
    {
        public CodeSet LanguageCode { get; set; }
        public CodeSet ModeCode { get; set; }
        public CodeSet ProficiencyLevelCode { get; set; }
        public bool PreferenceInd { get; set; }
    }

    public class BirthPlace
    {
        public Place Place { get; set; }
    }

    public class Place
    {
        public Address Address { get; set; }

    }

    public class Address
    {
        public CodeSet Country { get; set; }
        public CodeSet PostalCode { get; set; }
        public CodeSet State { get; set; }
    }

    public class Guardian
    {
        public CodeSet PersonalRelationshipRoleType { get; set; }

        public List<UsRealmAddress> Addresses { get; set; }

        public List<Telecom> Telecoms { get; set; }

        public GuardianPerson GuardianPerson { get; set; }
    }

    public class GuardianPerson
    {
        public List<UsRealPersonName> Names { get; set; } 
    }


    public class BirthTime
    {
        public string Year;
        public string Month;
        public string Day;
    }

    public class UsRealPersonName
    {
        public string[] Given { get; set; }
        public string Family { get; set; }
        public string[] Prefixes { get; set; }
        public string[] Suffixes { get; set; }
    }

    public class Telecom
    {
        public string Value { get; set; }
        public CodeSet Use { get; set; }
    }

    public class UsRealmAddress
    {
        public CodeSet PostalAddressValueSet { get; set; }
        
        public string Country { get; set; }
        
        public string State { get; set; }
        
        public string City { get; set; }
        
        public string PostalCode { get; set; }

        public string[] StreetAddressLine { get; set; }

    }
}
