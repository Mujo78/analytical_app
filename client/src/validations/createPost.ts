import * as Yup from "yup";

export const postType = {
  Question: 1,
  Answer: 2,
  Wiki: 3,
  TagWikiExerpt: 4,
  TagWiki: 5,
  ModeratorNomination: 6,
  WikiPlaceholder: 7,
  PrivilegeWiki: 8,
};

const postTypes = Object.values(postType);

export const createPostValidationSchema = Yup.object({
  title: Yup.string()
    .required("Title is required.")
    .max(250, "Title cannot exceed 250 characters.")
    .min(5, "Title must be at least 5 characters long."),
  body: Yup.string().required("Body is required"),
  postTypeId: Yup.number()
    .required("Post Type is required.")
    .oneOf(postTypes, "Post type must be valid."),
  tags: Yup.string().optional(),
});
