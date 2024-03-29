﻿using System.ComponentModel.DataAnnotations;

namespace NCBack.Dtos.User;

public class UserEditDto
{

    public int? CityId { get; set; } = null;
    public string? FullName { get; set; } = null;
    public string? CredoAboutMyself { get; set; } = null;
    public List<string>? LanguageOfCommunication { get; set; } = null;
    public string? Nationality { get; set; } = null;
    public int? GenderId { get; set; } = null;
    public string? MaritalStatus { get; set; } = null;
    public string? Сhildren { get; set; } = null;
    
    public string? GetAcquaintedWith { get; set; } = null;
    public string? MeetFor { get; set; } = null;
    public int? From { get; set; } = null;
    public int? To { get; set; } = null;
    public List<string>? Interests { get; set; } = null;
    //public List<string> PreferredPlaces { get; set; } =  new List<string> { string.Empty } ;
    
    /*public int? MeetingCategoryId { get; set; } = null;
    public int? MeatingPlaceId { get; set; }= null;
    public List<int>? MyInterestsId { get; set; }= null;
    public List<int>? MainСategoriesId { get; set; }= null;*/
    
    public string? IWantToLearn { get; set; } = null;
    public string? Profession { get; set; } = null;
}