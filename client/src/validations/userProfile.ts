import * as Yup from "yup";

export const profileDataValidationSchema = Yup.object({
  displayName: Yup.string()
    .required("Display name is required.")
    .max(40, "Display name cannot exceed 40 characters."),
  email: Yup.string()
    .email()
    .max(40, "Email cannot exceed 40 characters.")
    .optional(),
  location: Yup.string()
    .max(100, "Location name cannot exceed 100 characters.")
    .optional(),
  aboutMe: Yup.string().optional(),
  age: Yup.number().optional(),
  websiteUrl: Yup.string()
    .max(200, "WebsiteURL name cannot exceed 200 characters.")
    .optional(),
});
