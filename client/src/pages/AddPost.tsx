import { yupResolver } from "@hookform/resolvers/yup";
import {
  Button,
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { useMemo } from "react";
import { Controller, useForm } from "react-hook-form";
import { useParams } from "react-router";
import {
  createPostValidationSchema,
  postType,
} from "../validations/createPost";
import type { CreatePostType } from "../types/post.type";
import { CreatePost } from "../services/postService";

const AddPost = () => {
  const { orm, userId } = useParams();

  const {
    control,
    handleSubmit,
    formState: { errors, isDirty },
    reset,
  } = useForm({
    resolver: yupResolver(createPostValidationSchema),
  });

  const onSubmit = async (values: CreatePostType) => {
    if (orm && userId && isDirty) {
      const useDapper = orm === "dapper";
      await CreatePost(userId, values, useDapper);
      reset();
    }
  };

  const selectPostTypes = useMemo(() => {
    return Object.entries(postType).map(([key, value]) => ({
      label: key,
      value: value,
    }));
  }, []);

  console.log(selectPostTypes);
  return (
    <Stack gap={6}>
      <Typography variant="h6">Add new Post for Selected User</Typography>
      <Stack component="form" gap={3} onSubmit={handleSubmit(onSubmit)}>
        <Stack flexDirection="row" gap={2} flexWrap="wrap">
          <Controller
            control={control}
            name="title"
            defaultValue=""
            render={({ field }) => (
              <TextField
                {...field}
                sx={{ flexGrow: 1 }}
                label="Title"
                required
                type="text"
                error={!!errors.title}
                helperText={errors.title?.message}
              />
            )}
          />

          <Controller
            control={control}
            name="postTypeId"
            defaultValue={1}
            render={({ field }) => (
              <FormControl
                error={!!errors.postTypeId}
                required
                sx={{ flexGrow: 1 }}
              >
                <InputLabel id="select-post-type-label">Post Type</InputLabel>
                <Select
                  labelId="select-post-type-label"
                  {...field}
                  label="Post Type"
                  error={!!errors.postTypeId}
                >
                  {selectPostTypes.map((type) => (
                    <MenuItem value={type.value} key={type.value}>
                      {type.label}
                    </MenuItem>
                  ))}
                </Select>
                <FormHelperText>{errors.postTypeId?.message}</FormHelperText>
              </FormControl>
            )}
          />
        </Stack>
        <Controller
          control={control}
          name="body"
          defaultValue=""
          render={({ field }) => (
            <TextField
              {...field}
              sx={{ flexGrow: 1 }}
              label="Body"
              type="text"
              multiline
              error={!!errors.body}
              helperText={errors.body?.message}
            />
          )}
        />

        <Controller
          control={control}
          name="tags"
          defaultValue=""
          render={({ field }) => (
            <TextField
              {...field}
              label="Tags"
              type="text"
              error={!!errors.tags}
              helperText={errors.tags?.message}
            />
          )}
        />

        <Button type="submit" variant="contained">
          Submit
        </Button>
      </Stack>
    </Stack>
  );
};

export default AddPost;
