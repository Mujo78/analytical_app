import { useNavigate, useParams } from "react-router";
import { profileDataValidationSchema } from "../validations/userProfile";
import { Controller, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import type { UserProfileType } from "../types/user.type";
import { CreateUserProfile } from "../services/userService";
import { Button, Stack, TextField, Typography } from "@mui/material";

const AddUser = () => {
  const { orm } = useParams();
  const navigate = useNavigate();

  const {
    control,
    handleSubmit,
    formState: { errors, isDirty },
    reset,
  } = useForm({
    resolver: yupResolver(profileDataValidationSchema),
  });

  const onSubmit = async (values: UserProfileType) => {
    if (orm && isDirty) {
      const useDapper = orm === "dapper";
      await CreateUserProfile(values, useDapper);
      reset();
    }
  };

  const handleNavigateBack = () => {
    if (orm) {
      const ormToUse = orm === "dapper" ? "dapper" : "entity-core";
      navigate(`/${ormToUse}`);
    }
  };

  return (
    <Stack gap={6}>
      <Typography variant="h6">Create User Profile</Typography>
      <Stack component="form" gap={3} onSubmit={handleSubmit(onSubmit)}>
        <Stack flexDirection="row" gap={2} flexWrap="wrap">
          <Controller
            control={control}
            name="displayName"
            defaultValue=""
            render={({ field }) => (
              <TextField
                {...field}
                sx={{ flexGrow: 1 }}
                label="Username"
                required
                type="text"
                error={!!errors.displayName}
                helperText={errors.displayName?.message}
              />
            )}
          />

          <Controller
            control={control}
            name="email"
            defaultValue=""
            render={({ field }) => (
              <TextField
                {...field}
                sx={{ flexGrow: 1 }}
                label="Email"
                type="text"
                error={!!errors.email}
                helperText={errors.email?.message}
              />
            )}
          />
        </Stack>

        <Stack flexDirection="row" gap={2} flexWrap="wrap">
          <Controller
            control={control}
            name="location"
            defaultValue=""
            render={({ field }) => (
              <TextField
                {...field}
                sx={{ flexGrow: 1 }}
                label="Location"
                type="text"
                error={!!errors.location}
                helperText={errors.location?.message}
              />
            )}
          />

          <Controller
            control={control}
            name="age"
            defaultValue={0}
            render={({ field }) => (
              <TextField
                {...field}
                label="Age"
                type="number"
                error={!!errors.age}
                helperText={errors.age?.message}
              />
            )}
          />
        </Stack>

        <Controller
          control={control}
          name="websiteUrl"
          defaultValue=""
          render={({ field }) => (
            <TextField
              {...field}
              label="Website URL"
              type="text"
              error={!!errors.websiteUrl}
              helperText={errors.websiteUrl?.message}
            />
          )}
        />

        <Controller
          control={control}
          name="aboutMe"
          defaultValue=""
          render={({ field }) => (
            <TextField
              {...field}
              sx={{ flexGrow: 1 }}
              label="About Me"
              multiline
              type="text"
              error={!!errors.aboutMe}
              helperText={errors.aboutMe?.message}
            />
          )}
        />

        <Stack flexDirection="row" justifyContent="space-between">
          <Button onClick={handleNavigateBack}>Back</Button>
          <Button type="submit" variant="contained">
            Submit
          </Button>
        </Stack>
      </Stack>
    </Stack>
  );
};

export default AddUser;
